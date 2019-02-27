using Celia.io.Core.Auths.Abstractions;
using Celia.io.Core.Auths.Abstractions.Exceptions;
using Celia.io.Core.Auths.Abstractions.ResponseDTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Celia.io.Core.Auths.SDK
{
    public class RoleManager : ApiHelperBase
    {
        public RoleManager(string appId, string appSecret, string authApiHost)
            : base(appId, appSecret, authApiHost)
        {
        }

        /// <summary>
        /// 根据角色ID查找角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public async Task<RoleResponseResult> FindRoleByIdAsync(string token, string roleId)
        {
            try
            {
                JObject response = await this.TokenHttpGetAsync(token,
                "api/roles/findbyid?roleId=" + roleId);
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<RoleResponseResult>(response.ToString());
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("RoleManager.FindRoleByIdAsync exception", ex);
            }
        }

        /// <summary>
        /// 根据角色名查找角色
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <returns></returns>
        public async Task<RoleResponseResult> FindRoleByNameAsync(string token, string roleName)
        {
            try
            {
                JObject response = await this.TokenHttpGetAsync(token,
                    "api/roles/findbyname?roleName=" + roleName);
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<RoleResponseResult>(response.ToString());
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("RoleManager.FindRoleByNameAsync exception", ex);
            }
        }

        /// <summary>
        /// 根据一个用户的Id，获取对应的所有角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<RolesResponseResult> GetRolesByUserIdAsync(string token, string userId)
        {
            try
            {
                JObject response = await this.TokenHttpGetAsync(token,
                "api/users/getrolesbyuserid?userId=" + userId);
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<RolesResponseResult>(response.ToString());
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("RoleManager.GetRolesByUserIdAsync exception", ex);
            }
        }

        /// <summary>
        /// 更新一个角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<RoleResponseResult> UpdateRoleAsync(string token, ApplicationRole role)
        {
            try
            {
                JObject response = await this.TokenHttpPostAsync(token,
                "api/roles/update", JObject.FromObject(role));

                if (response != null)
                {
                    return JsonConvert.DeserializeObject<RoleResponseResult>(response.ToString());
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("RoleManager.UpdateRoleAsync exception", ex);
            }
        }

        /// <summary>
        /// 添加一个角色
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <returns></returns>
        public async Task<RoleResponseResult> AddRoleAsync(string token, string roleName)
        {
            try
            {
                ApplicationRole role = new ApplicationRole()
                {
                    Name = roleName
                };
                JObject response = await this.TokenHttpPostAsync(token,
                    "api/roles/create", JObject.FromObject(role));
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<RoleResponseResult>(response.ToString());
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("RoleManager.AddRoleAsync exception", ex);
            }
        }

        /// <summary>
        /// 删除一个角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        public async Task<ActionResponseResult> RemoveRoleAsync(string token, string roleId)
        {
            try
            {
                ApplicationRole role = new ApplicationRole()
                {
                    Id = roleId
                };
                JObject response = await this.TokenHttpPostAsync(token,
                    "api/roles/delete", JObject.FromObject(role));
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<ActionResponseResult>(response.ToString());
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("RoleManager.RemoveRoleAsync exception", ex);
            }
        }

        /// <summary>
        /// 添加单个权限到角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="claimType">权限类型</param>
        /// <param name="claimValue">权限值</param>
        /// <returns></returns>
        public async Task<RoleClaimResponseResult> AddRoleClaimAsync(string token,
            string roleId, string claimType, string claimValue)
        {
            try
            {
                JObject response = await this.TokenHttpPostAsync(token,
                "api/roleclaims/addRoleClaim", JObject.FromObject(
                    new ApplicationRoleClaim()
                    {
                        RoleId = roleId,
                        ClaimType = claimType,
                        ClaimValue = claimValue,
                    }));
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<RoleClaimResponseResult>(response.ToString());
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("RoleManager.AddRoleClaimAsync exception", ex);
            }
        }

        /// <summary>
        /// 删除一个角色的权限
        /// </summary>
        /// <param name="roleClaim"></param>
        public async Task<ActionResponseResult> RemoveRoleClaimAsync(string token, ApplicationRoleClaim roleClaim)
        {
            try
            {
                JObject response = await this.TokenHttpPostAsync(token,
                    "api/roleclaims/removeRoleClaim", JObject.FromObject(roleClaim));
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<ActionResponseResult>(
                        response.ToString());
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("RoleManager.RemoveRoleClaimAsync exception", ex);
            }
        }

        /// <summary>
        /// 根据角色ID，获取对应的所有权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<RoleClaimsResponseResult> GetClaimsByRoleIdAsync(
            string token, string roleId)
        {
            try
            {
                JObject response = await this.TokenHttpGetAsync(token,
                    "api/roles/getclaimsbyroleid?roleId=" + roleId);
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<RoleClaimsResponseResult>(
                        response.ToString());
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("RoleManager.GetClaimsByRoleIdAsync exception", ex);
            }
        }

        /// <summary>
        /// 判断用户是否属于某个角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public async Task<bool> IsInRoleAsync(string userId, string roleName)
        {
            JObject response = await this.HttpGetAsync(
                $"api/userroles/isinrole?userId={userId}&roleName={roleName}");
            if (response != null)
            {
                return response.Value<bool>("result");
            }

            return false;
        }

        /// <summary>
        /// 添加权限（Claims）到某个角色
        /// </summary>
        /// <param name="claims">权限列表</param>
        /// <returns></returns>
        public async Task<RoleClaimsResponseResult> AddClaimsToRoleAsync(
            string token, IEnumerable<ApplicationRoleClaim> claims)
        {
            try
            {
                JArray array = new JArray();
                foreach (var c in claims)
                {
                    array.Add(JObject.FromObject(c));
                }

                JObject response = await this.TokenHttpPostAsync(token, "api/roles/addclaimstorole", array);
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<RoleClaimsResponseResult>(
                        response.ToString());
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("RoleManager.AddClaimsToRoleAsync exception", ex);
            }
        }
    }
}
