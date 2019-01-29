using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Celia.io.Core.Auths.Abstractions;
using Celia.io.Core.Auths.Abstractions.ResponseDTOs;
using Celia.io.Core.Auths.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Celia.io.Core.Auths.WebAPI_Core.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly ILogger<UserRolesController> _logger;
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationRoleManager _roleManager;

        public UserRolesController(ILogger<UserRolesController> logger,
            ApplicationUserManager userManager,
            ApplicationRoleManager roleManager)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this._roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        // GET: api/UserRoles
        [HttpPost("adduserroles")]
        [AllowAnonymous]
        public async Task<UserRolesResponseResult> AddUserRoles(
            [FromBody] IEnumerable<ApplicationUserRole> userRoles)
        {
            if (userRoles == null || userRoles.Count() < 1)
            {
                return new UserRolesResponseResult()
                {
                    Code = 200,
                    Data = new ApplicationUserRole[] { }
                };
            }

            return await _userManager.FindByIdAsync(userRoles.First().UserId)
                .ContinueWith<UserRolesResponseResult>((m) =>
                {
                    if (m.IsCompleted && !m.IsFaulted && m.Result != null)
                    {
                        IEnumerable<string> roles = from one in userRoles
                                                    select one.RoleId;

                        var roleNames = _roleManager.Roles.Where(m1 => roles.Contains(m1.Id))
                            .Select(m2 => m2.Name);

                        var result = _userManager.AddToRolesAsync(m.Result, roleNames);
                        result.Wait();
                        if (result.IsCompleted && !result.IsFaulted && result.Result.Succeeded)
                        {
                            return new UserRolesResponseResult() { Code = 200, Data = userRoles.ToArray() };
                        }
                    }

                    return new UserRolesResponseResult()
                    {
                        Code = 200,
                        Data = new ApplicationUserRole[] { }
                    };
                });
        }

        // POST: api/UserRoles
        [HttpPost("adduserrole")]
        [AllowAnonymous]
        public async Task<UserRoleResponseResult> AddUserRole(
            [FromBody] ApplicationUserRole userRole)
        {
            try
            {
                if (userRole != null && !string.IsNullOrEmpty(userRole.UserId)
                    && !string.IsNullOrEmpty(userRole.RoleId))
                {
                    ApplicationUser user = await _userManager.FindByIdAsync(userRole.UserId);
                    if (user != null)
                    {
                        if (await _userManager.IsInRoleAsync(user, userRole.RoleId))
                        {
                            return new UserRoleResponseResult()
                            {
                                Code = 200,
                                Data = userRole
                            };
                        }

                        var role = await this._roleManager.FindByIdAsync(userRole.RoleId);
                        if (role != null)
                        {
                            var result = await _userManager.AddToRoleAsync(user, role.Name);

                            if (result.Succeeded || result.Errors?.FirstOrDefault()?.Code == "UserAlreadyInRole")
                            {
                                return new UserRoleResponseResult()
                                {
                                    Code = 200,
                                    Data = userRole
                                };
                            }
                            else
                            {
                                return new UserRoleResponseResult()
                                {
                                    Code = 403,
                                    Message = result.Errors.FirstOrDefault()?.Description,
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UserRolesController.adduserrole", userRole);
            }

            return new UserRoleResponseResult()
            {
                Code = 400,
                Message = "Role does not exist. ",
            };
        }

        // DELETE: api/ApiWithActions/5
        [HttpPost("removeuserrole")]
        public async Task<ActionResponseResult> RemoveUserRole([FromBody] ApplicationUserRole userRole)
        {
            try
            {
                if (userRole == null || string.IsNullOrEmpty(userRole.UserId))
                    return new ActionResponseResult() { Code = 200 };

                ApplicationUser user = await _userManager.FindByIdAsync(userRole.UserId);

                if (user == null)
                {
                    return new ActionResponseResult()
                    {
                        Code = 400,
                        Message = "User does not exist. ",
                    };
                }

                var role = await this._roleManager.FindByIdAsync(userRole.RoleId);
                if (role != null)
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, role.Name)
                        .ContinueWith((r) =>
                        {
                            if (!r.IsFaulted && r.Result.Succeeded)
                            {
                                return new ActionResponseResult()
                                {
                                    Code = 200,
                                };
                            }

                            return new ActionResponseResult()
                            {
                                Code = 403,
                                Message = r.Result?.Errors?.FirstOrDefault()?.Description,// r.Exception?.Message,
                            };
                        });

                    return result;
                }

                return new ActionResponseResult()
                {
                    Code = 400,
                    Message = "Role does not exist. ",
                };
            }
            catch (Exception ex)
            {
                return new ActionResponseResult()
                {
                    Code = 500,
                    Message = ex.Message,
                };
            }
        }

        // GET: api/UserRoles
        [HttpPost("removeuserroles")]
        public async Task<ActionResponseResult> RemoveUserRoles(
            [FromBody] IEnumerable<ApplicationUserRole> userRoles)
        {
            try
            {
                if (userRoles == null || userRoles.Count() < 1)
                    return new ActionResponseResult() { Code = 200 };

                ApplicationUser user = await _userManager.FindByIdAsync(userRoles.First().UserId);

                if (user == null)
                {
                    return new ActionResponseResult()
                    {
                        Code = 400,
                        Message = "User does not exist. ",
                    };
                }

                IEnumerable<string> roles = from one in userRoles
                                            select one.RoleId;
                IEnumerable<string> roleNames = _roleManager.Roles.Where(m1 => roles.Contains(m1.Id))
                    .Select(m2 => m2.Name);

                return await _userManager.RemoveFromRolesAsync(user, roleNames)
                    .ContinueWith((r) =>
                    {
                        if (!r.IsFaulted && r.Result.Succeeded)
                        {
                            return new ActionResponseResult()
                            {
                                Code = 200,
                            };
                        }

                        return new ActionResponseResult()
                        {
                            Code = 403,
                            Message = r.Result?.Errors?.FirstOrDefault()?.Description,// r.Exception?.Message,
                        };
                    });
            }
            catch (Exception ex)
            {
                return new ActionResponseResult()
                {
                    Code = 500,
                    Message = ex.Message,
                };
            }
        }

        [HttpPost("removeuserallroles")]
        public async Task<ActionResponseResult> RemoveUserAllRoles(
            [FromBody] ApplicationUser user)
        {
            if (user != null && !string.IsNullOrEmpty(user.Id))
            {
                user = await _userManager.FindByIdAsync(user.Id);
                if (user == null)
                {
                    return new ActionResponseResult()
                    {
                        Code = 400,
                        Message = "User does not exist. ",
                    };
                }

                return await _userManager.GetRolesAsync(user)
                    .ContinueWith<ActionResponseResult>((m) =>
                    {
                        if (m.IsCompleted && !m.IsFaulted && m.Result != null && m.Result.Count > 0)
                        {
                            var task = _userManager.RemoveFromRolesAsync(user, m.Result);
                            task.Wait();

                            if (!task.IsFaulted)
                            {
                                return new ActionResponseResult()
                                {
                                    Code = 200,
                                };
                            }

                            return new ActionResponseResult()
                            {
                                Code = 403,
                                Message = task.Result?.Errors?.FirstOrDefault()?.Description,
                            };
                        }
                        else
                        {
                            if (!m.IsFaulted)
                            {
                                return new ActionResponseResult()
                                {
                                    Code = 200,
                                };
                            }

                            return new ActionResponseResult()
                            {
                                Code = 403,
                                Message = m.Exception?.Message,
                            };
                        }
                    });
            }

            return new ActionResponseResult() { Code = 200 };
        }

        [HttpGet("isinrole")]
        [AllowAnonymous]
        public async Task<bool> IsInRole([FromQuery] [Required] string userId,
            [FromQuery] [Required] string roleName)
        {
            return await _userManager.IsInRoleAsync(new ApplicationUser() { Id = userId }, roleName);
        }
    }
}
