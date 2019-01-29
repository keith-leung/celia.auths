using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Celia.io.Core.Auths.Abstractions;
using Celia.io.Core.Auths.Abstractions.ResponseDTOs;
using Celia.io.Core.Auths.Services;
using Celia.io.Core.Auths.WebAPI_Core.Models; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections;

namespace Celia.io.Core.Auths.WebAPI_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ILogger<RolesController> _logger;
        private readonly ApplicationRoleManager _roleManager;

        public RolesController(ILogger<RolesController> logger, ApplicationRoleManager roleManager)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        [HttpGet("test")]
        public async Task<string> test([FromQuery] string tester)
        {
            return $"{tester} action received. ";
        }

        // GET: api/Roles/list
        [HttpGet("list")]
        public async Task<RolesResponseResult> List([FromQuery] SimpleSort simpleSort)
        {
            try
            {
                int pageSize2 = 20;
                if (simpleSort.PageSize.HasValue)
                    pageSize2 = simpleSort.PageSize.Value;

                if (simpleSort.OrderType.HasValue && simpleSort.OrderType.Value != 0)
                {
                    return new RolesResponseResult()
                    {
                        Code = 200,
                        Data = (this._roleManager.Roles.OrderByDescending(m => m.NormalizedName)
                        .Skip((simpleSort.PageIndex - 1) * pageSize2).Take(pageSize2)
                        ).ToArray()
                    };
                }

                return new RolesResponseResult()
                {
                    Code = 200,
                    Data = (this._roleManager.Roles.OrderBy(m => m.NormalizedName)
                        .Skip((simpleSort.PageIndex - 1) * pageSize2).Take(pageSize2)
                        ).ToArray()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RolesController.list", simpleSort);
                return new RolesResponseResult()
                {
                    Code = 500,
                    Message = ex.Message,
                };
            }
        }

        // GET: api/Roles/findbyid/{id}
        [HttpGet("findbyid")]
        public async Task<RoleResponseResult> FindById([FromQuery] [Required] string roleId)
        {
            return await this._roleManager.FindByIdAsync(roleId)
                .ContinueWith((r) =>
                {
                    if (r.IsCompleted && !r.IsFaulted)
                    {
                        return new RoleResponseResult() { Code = 200, Data = r.Result };
                    }
                    return new RoleResponseResult() { Code = 400, Message = r.Exception?.Message };
                });
        }

        [HttpGet("findbyname")]
        public async Task<RoleResponseResult> FindByName([FromQuery] [Required] string roleName)
        {
            return await this._roleManager.FindByNameAsync(roleName)
                .ContinueWith((r) =>
                {
                    if (r.IsCompleted && !r.IsFaulted)
                    {
                        return new RoleResponseResult() { Code = 200, Data = r.Result };
                    }
                    return new RoleResponseResult() { Code = 400, Message = r.Exception?.Message };
                });
        }

        [HttpPost("create")]
        public async Task<RoleResponseResult> Create([FromBody] ApplicationRole role)
        {
            return await this._roleManager.CreateAsync(role)
                .ContinueWith<RoleResponseResult>((r) =>
                {
                    if (r.IsCompleted && !r.IsFaulted && r.Result.Succeeded)
                    {
                        var result2 = _roleManager.FindByNameAsync(role.Name);
                        result2.Wait();

                        return new RoleResponseResult()
                        {
                            Code = 200,
                            Data = result2.Result
                        };
                    }
                    else if (r.IsFaulted)
                    {
                        return new RoleResponseResult()
                        {
                            Code = 400,
                            Message = r.Exception?.Message,
                        };
                    }
                    else if (!r.Result.Succeeded)
                    {
                        return new RoleResponseResult()
                        {
                            Code = (int)System.Net.HttpStatusCode.ExpectationFailed,
                            Message = r.Result.Errors?.FirstOrDefault()?.Description
                        };
                    }

                    return new RoleResponseResult() { Code = 200 };//, Data = r.Result };
                });
        }

        [HttpPost("update")]
        public async Task<ActionResponseResult> Update([FromBody] ApplicationRole role)
        {
            return await this._roleManager.UpdateAsync(role)
                .ContinueWith<ActionResponseResult>((r) =>
                {
                    if (r.IsCompleted && !r.IsFaulted && r.Result.Succeeded)
                    {
                        return new ActionResponseResult()
                        {
                            Code = 200,
                        };
                    }
                    else if (r.IsFaulted)
                    {
                        return new ActionResponseResult()
                        {
                            Code = 400,
                            Message = r.Exception?.Message,
                        };
                    }
                    else if (!r.Result.Succeeded)
                    {
                        return new ActionResponseResult()
                        {
                            Code = (int)System.Net.HttpStatusCode.ExpectationFailed,
                            Message = r.Result.Errors?.FirstOrDefault()?.Description
                        };
                    }

                    return new ActionResponseResult() { Code = 200 };
                });
        }

        [HttpPost("delete")]
        public async Task<ActionResponseResult> Delete([FromBody] ApplicationRole role)
        {
            return await this._roleManager.DeleteAsync(role)
                .ContinueWith<ActionResponseResult>((r) =>
                {
                    if (r.IsCompleted && !r.IsFaulted && r.Result.Succeeded)
                    {
                        return new ActionResponseResult()
                        {
                            Code = 200,
                        };
                    }
                    else if (r.IsFaulted)
                    {
                        return new ActionResponseResult()
                        {
                            Code = 400,
                            Message = r.Exception?.Message,
                        };
                    }
                    else if (!r.Result.Succeeded)
                    {
                        return new ActionResponseResult()
                        {
                            Code = (int)System.Net.HttpStatusCode.ExpectationFailed,
                            Message = r.Result.Errors?.FirstOrDefault()?.Description
                        };
                    }

                    return new ActionResponseResult() { Code = 200 };
                });
        }

        [HttpPost("addclaimstorole")]
        public async Task<RoleClaimsResponseResult> AddClaimsToRole(
            [FromBody] IEnumerable<ApplicationRoleClaim> roleClaims)
        {
            foreach (var rc in roleClaims)
            {
                var res1 = await _roleManager.AddClaimAsync(
                    new ApplicationRole()
                    {
                        Id = rc.RoleId
                    },
                    new System.Security.Claims.Claim(
                        rc.ClaimType,
                        rc.ClaimValue));

                if (!res1.Succeeded)
                {
                    return new RoleClaimsResponseResult()
                    {
                        Code = 400,
                        Message = res1.Errors?.FirstOrDefault()?.Description
                    };
                }
            }

            return new RoleClaimsResponseResult()
            {
                Code = 400,
                Data = roleClaims.ToArray()
            };
        }

        [HttpGet("getclaimsbyroleid")]
        public async Task<RoleClaimsResponseResult> GetClaimsByRoleId(
            [FromQuery] [Required] string roleId)
        {
            ApplicationRole role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                var result = await _roleManager.GetClaimsAsync(role);
                return new RoleClaimsResponseResult()
                {
                    Code = 200,
                    Data = (from one in result
                            select new ApplicationRoleClaim()
                            {
                                RoleId = roleId,
                                ClaimType = one.Type,
                                ClaimValue = one.Value
                            }).ToArray()
                };
            }

            return new RoleClaimsResponseResult()
            {
                Code = 403,
                Message = "Role does not exists"
            };
        }
    }
}
