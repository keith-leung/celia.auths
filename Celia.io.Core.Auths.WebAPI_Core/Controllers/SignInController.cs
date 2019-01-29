﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using Celia.io.Core.Auths.Abstractions.ResponseDTOs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celia.io.Core.Auths.Abstractions;
using Celia.io.Core.Auths.Services;
using Celia.io.Core.Auths.WebAPI_Core.Models;
using Celia.io.Core.MicroServices.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Celia.io.Core.Auths.WebAPI_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SignInController : ControllerBase
    {
        private readonly ILogger<SignInController> _logger;
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly DisconfService _disconfService;
        private readonly SigningCredentials _signingCredentials;

        private readonly IServiceProvider _serviceProvider;

        public string Issuer { get; set; }
        public string Audience { get; set; }

        public SignInController(IServiceProvider serviceProvider,
            ILogger<SignInController> logger, ApplicationUserManager userManager,
            ApplicationSignInManager signInManager)//, SigningCredentials signingCredentials)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this._signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));

            this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _signingCredentials = serviceProvider.GetService(typeof(SigningCredentials)) as SigningCredentials;
            _disconfService = serviceProvider.GetService(typeof(DisconfService)) as DisconfService;
        }

        [HttpGet("accesstoken")]
        public async Task<TokenResponseResult> AccessToken()
        {
            try
            {
                DateTimeOffset offset = new DateTimeOffset(DateTime.Now);
                offset = offset.AddDays(7); //7天内过期 

                return new TokenResponseResult()
                {
                    Code = 200,
                    AccessToken = _signInManager.GetToken(Issuer, Audience, _signingCredentials,
                    HttpContext.User.Identity.Name, HttpContext.User.Claims, offset)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SignInController.accesstoken");
                return new TokenResponseResult()
                {
                    Code = (int)System.Net.HttpStatusCode.ExpectationFailed,
                    Message = ex.Message,
                };
            }
        }

        //[AllowAnonymous]
        [HttpPost("refreshtoken")]
        public async Task<TokenResponseResult> RefreshToken([FromBody] LoginRequest request)
        {
            try
            {
                DateTimeOffset offset = new DateTimeOffset(DateTime.Now);
                offset = offset.AddDays(7); //7天内过期 
                string userId = request.Key;
                string token = request.Value;
                return new TokenResponseResult()
                {
                    Code = 200,
                    AccessToken = _signInManager.RefreshToken(token, Issuer, Audience, _signingCredentials,
                          userId, HttpContext.User.Claims, offset)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SignInController.refreshtoken");
                return new TokenResponseResult()
                {
                    Code = (int)System.Net.HttpStatusCode.ExpectationFailed,
                    Message = ex.Message,
                };
            }
        }

        [HttpGet("validatetoken")]
        public async Task<TokenResponseResult> ValidateToken([FromQuery] [Required] string token)
        {
            try
            {
                var response = new TokenResponseResult();

                if (_signInManager.ValidateToken(token))
                {
                    response.Code = 200;
                    response.AccessToken = token;

                    return response;
                }

                response.Code = 401;
                response.Message = "Token is invalid.";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SignInController.refreshtoken");
                return new TokenResponseResult()
                {
                    Code = (int)System.Net.HttpStatusCode.ExpectationFailed,
                    Message = ex.Message,
                };
            }
        }

        [HttpPost("loginbyusername")]
        [AllowAnonymous]
        public async Task<LoginResponseResult> LoginByUserName([FromBody] LoginRequest request)
        {
            var username = request.Key;
            string password = request.Value;
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, password,
                    false, false);
                if (signInResult.Succeeded)
                {
                    string token = await this.GetToken(user);

                    return new LoginResponseResult()
                    {
                        Code = 200,
                        AccessToken = token,
                        Data = user
                    };
                }
                else if (signInResult.IsLockedOut)
                {
                    return new LoginResponseResult()
                    {
                        Code = (int)System.Net.HttpStatusCode.Locked, //423
                        Message = "User is locked. Please contact Administrator for unlock. "
                    };
                }
                else if (signInResult.IsNotAllowed)
                {
                    return new LoginResponseResult()
                    {
                        Code = (int)System.Net.HttpStatusCode.MethodNotAllowed, //405
                        Message = "User is not allowed. "
                    };
                }
                else if (signInResult.RequiresTwoFactor)
                {
                    return new LoginResponseResult()
                    {
                        Code = (int)System.Net.HttpStatusCode.ExpectationFailed, //417
                        Message = "Two-Factor sign-in is required. "
                    };
                }
            }

            return new LoginResponseResult() { Code = 401 };
        }

        private async Task<string> GetToken(ApplicationUser user)
        {
            var claimPrincipals = _signInManager.ClaimsFactory.CreateAsync(user);

            try
            {
                DateTimeOffset offset = new DateTimeOffset(DateTime.Now);
                offset = offset.AddDays(7); //7天内过期 

                string token = _signInManager.GetToken(_disconfService.CustomConfigs["Issuer"].ToString(),
                    _disconfService.CustomConfigs["Audience"].ToString(),
                    _signingCredentials,
                    user.Id, await _userManager.GetClaimsAsync(user), //创建用户跟权限的Claim
                    offset);

                return token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("loginbyuserid")]
        [AllowAnonymous]
        public async Task<LoginResponseResult> LoginByUserId([FromBody] LoginRequest request)
        {
            var userid = request.Key;
            string password = request.Value;

            var user = await _userManager.FindByIdAsync(userid);
            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user.UserName, password,
                    false, false);
                if (signInResult.Succeeded)
                {
                    string token = await this.GetToken(user);

                    return new LoginResponseResult()
                    {
                        Code = 200,
                        AccessToken = token,
                        Data = user
                    };
                }
                else if (signInResult.IsLockedOut)
                {
                    return new LoginResponseResult()
                    {
                        Code = (int)System.Net.HttpStatusCode.Locked, //423
                        Message = "User is locked. Please contact Administrator for unlock. "
                    };
                }
                else if (signInResult.IsNotAllowed)
                {
                    return new LoginResponseResult()
                    {
                        Code = (int)System.Net.HttpStatusCode.MethodNotAllowed, //405
                        Message = "User is not allowed. "
                    };
                }
                else if (signInResult.RequiresTwoFactor)
                {
                    return new LoginResponseResult()
                    {
                        Code = (int)System.Net.HttpStatusCode.ExpectationFailed, //417
                        Message = "Two-Factor sign-in is required. "
                    };
                }
            }

            return new LoginResponseResult() { Code = 401 };
        }

        [HttpPost("loginbyemail")]
        [AllowAnonymous]
        public async Task<LoginResponseResult> LoginByEmail([FromBody] LoginRequest request)
        {
            var email = request.Key;
            string password = request.Value;

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, password,
                    false, false);
                if (signInResult.Succeeded)
                {
                    string token = await this.GetToken(user);

                    return new LoginResponseResult()
                    {
                        Code = 200,
                        AccessToken = token,
                        Data = user
                    };
                }
                else if (signInResult.IsLockedOut)
                {
                    return new LoginResponseResult()
                    {
                        Code = (int)System.Net.HttpStatusCode.Locked, //423
                        Message = "User is locked. Please contact Administrator for unlock. "
                    };
                }
                else if (signInResult.IsNotAllowed)
                {
                    return new LoginResponseResult()
                    {
                        Code = (int)System.Net.HttpStatusCode.MethodNotAllowed, //405
                        Message = "User is not allowed. "
                    };
                }
                else if (signInResult.RequiresTwoFactor)
                {
                    return new LoginResponseResult()
                    {
                        Code = (int)System.Net.HttpStatusCode.ExpectationFailed, //417
                        Message = "Two-Factor sign-in is required. "
                    };
                }
            }

            return new LoginResponseResult() { Code = 401 };
        }

        [HttpPost("externallogin")]
        [AllowAnonymous]
        public async Task<LoginResponseResult> ExternalLogin([FromBody] LoginRequest request)
        {
            try
            {
                var loginProvider = request.Key;
                string providerKey = request.Value;
                _logger.LogInformation($"loginProvider:{loginProvider},providerKey:{providerKey}");

                var signInResult = await _signInManager.ExternalLoginSignInAsync(
                    loginProvider, providerKey, false);
                _logger.LogInformation($"signInResult:{signInResult.Succeeded}");

                if (signInResult.Succeeded)
                {
                    var user = await _userManager.FindByLoginAsync(loginProvider, providerKey);
                    string token = await this.GetToken(user);
                    _logger.LogInformation($"tokenresponse:{token}");

                    return new LoginResponseResult()
                    {
                        Code = 200,
                        AccessToken = token,
                        Data = user
                    };
                }
                else if (signInResult.IsLockedOut)
                {
                    return new LoginResponseResult()
                    {
                        Code = (int)System.Net.HttpStatusCode.Locked, //423
                        Message = "User is locked. Please contact Administrator for unlock. "
                    };
                }
                else if (signInResult.IsNotAllowed)
                {
                    return new LoginResponseResult()
                    {
                        Code = (int)System.Net.HttpStatusCode.MethodNotAllowed, //405
                        Message = "User is not allowed. "
                    };
                }
                else if (signInResult.RequiresTwoFactor)
                {
                    return new LoginResponseResult()
                    {
                        Code = (int)System.Net.HttpStatusCode.ExpectationFailed, //417
                        Message = "Two-Factor sign-in is required. "
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SignInController.externallogin", request);
            }

            return new LoginResponseResult() { Code = 401 };
        }

        // DELETE: api/ApiWithActions/5
        [HttpPost("signout")]
        public async Task<ActionResponseResult> SignOut()
        {
            return await _signInManager.SignOutAsync()
                .ContinueWith((o) =>
                {
                    return new ActionResponseResult() { Code = 200 };
                });
        }
    }
}
