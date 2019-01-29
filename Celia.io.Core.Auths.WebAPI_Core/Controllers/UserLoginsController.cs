using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Celia.io.Core.Auths.Abstractions;
using Celia.io.Core.Auths.Abstractions.ResponseDTOs;
using Celia.io.Core.Auths.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Celia.io.Core.Auths.WebAPI_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginsController : ControllerBase
    {
        private readonly ILogger<UserLoginsController> _logger;
        private readonly ApplicationUserManager _userManager;

        public UserLoginsController(ILogger<UserLoginsController> logger, ApplicationUserManager userManager)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        // GET: api/UserLogins
        [HttpPost("adduserlogin")]
        public async Task<UserLoginResponseResult> AddUserLogin([FromBody] ApplicationUserLogin userLogin)
        {
            if (userLogin != null && !string.IsNullOrEmpty(userLogin.UserId))
            {
                var user = await _userManager.FindByIdAsync(userLogin.UserId);
                if (user != null)
                {
                    var result = await this._userManager.AddLoginAsync(user,
                         new UserLoginInfo(userLogin.LoginProvider, userLogin.ProviderKey, userLogin.ProviderDisplayName));
                    if (result.Succeeded || (result.Errors != null &&
                        (result.Errors.Count() > 0 && result.Errors.First().Code.Equals("LoginAlreadyAssociated"))))
                        return new UserLoginResponseResult() { Code = 200, Data = userLogin };

                    if (!result.Succeeded)
                    {
                        return new UserLoginResponseResult()
                        {
                            Code = (int)System.Net.HttpStatusCode.NotAcceptable,
                            Message = result.Errors?.FirstOrDefault()?.Description
                        };
                    }
                }
            }

            return new UserLoginResponseResult() { Code = 400, Message = "Parameter is invalid." };
        }

        // GET: api/UserLogins/5
        [HttpPost("removeuserlogin")]
        public async Task<ActionResponseResult> RemoveUserLogin([FromBody] ApplicationUserLogin userLogin)
        {
            if (userLogin != null)
            {
                return await this._userManager.RemoveLoginAsync(
                    new ApplicationUser() { Id = userLogin.UserId },
                    userLogin.LoginProvider, userLogin.ProviderKey)
                    .ContinueWith((m) =>
                    {
                        return new ActionResponseResult() { Code = 200 };
                    });
            }

            return new ActionResponseResult() { Code = 400, Message = "Parameter is invalid." };
        }

        // POST: api/UserLogins
        [HttpGet("getloginsbyuserid")]
        public async Task<UserLoginsResponseResult> GetLoginsByUserId([FromQuery] [Required] string userId)
        {
            var result = await _userManager.GetLoginsAsync(new ApplicationUser() { Id = userId });

            if (result != null && result.Count > 0)
            {
                var list = from one in result
                           select new ApplicationUserLogin()
                           {
                               UserId = userId,
                               LoginProvider = one.LoginProvider,
                               ProviderKey = one.ProviderKey,
                               ProviderDisplayName = one.ProviderDisplayName
                           };

                return new UserLoginsResponseResult() { Code = 200, Data = list.ToArray() };
            }

            return new UserLoginsResponseResult { Code = 200 };
        }

        // PUT: api/UserLogins/5
        [HttpGet("getloginbyuseridlogintype")]
        public async Task<UserLoginResponseResult> GetLoginByUserIdLoginType(
            [FromQuery] [Required] string userId, [FromQuery] string loginProvider
            , [FromQuery] string providerKey)
        {
            var result = await _userManager.GetLoginsAsync(new ApplicationUser() { Id = userId });
            if (result != null && result.Count > 0)
            {
                var result2 = result.FirstOrDefault(m =>
                m.LoginProvider.Equals(loginProvider, StringComparison.InvariantCultureIgnoreCase)
                && m.ProviderKey.Equals(providerKey, StringComparison.InvariantCultureIgnoreCase));
                if (result2 != null)
                {
                    return new UserLoginResponseResult()
                    {
                        Code = 200,
                        Data = new ApplicationUserLogin()
                        {
                            UserId = userId,
                            LoginProvider = result2.LoginProvider,
                            ProviderKey = result2.ProviderKey,
                            ProviderDisplayName = result2.ProviderDisplayName
                        }
                    };
                }
            }

            return new UserLoginResponseResult() { Code = 200 };
        }

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
