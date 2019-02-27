using System;
using System.Collections.Generic;
using System.Text;
using Celia.io.Core.Auths.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http; 
using System.Threading.Tasks;
using Celia.io.Core.Auths.Abstractions.ResponseDTOs;
using Celia.io.Core.Auths.Abstractions.Exceptions;

namespace Celia.io.Core.Auths.SDK
{
    public class UserManager : ApiHelperBase
    { 
        public UserManager(string appId, string appSecret, string authApiHost)
            : base(appId, appSecret, authApiHost)
        {
        }

        public async Task<ApplicationUser> FindUserByIdAsync(string userid)
        {
            JObject response = await this.HttpGetAsync($"api/users/findbyid?userid={userid}");

            if (response != null)
            {
                ApplicationUser user = JsonConvert.DeserializeObject<ApplicationUser>(response.ToString());

                return user;
            }

            return null;
        }

        public async Task<ApplicationUser> FindUserByUserName(string username)
        {
            JObject response = await this.HttpGetAsync($"api/users/findbyname?name={username}");

            if (response != null)
            {
                ApplicationUser user = JsonConvert.DeserializeObject<ApplicationUser>(response.ToString());

                return user;
            }

            return null;
        }

        public async Task<ApplicationUser> FindUserByEmail(string email)
        {
            JObject response = await this.HttpGetAsync($"api/users/findbyemail?email={email}");

            if (response != null)
            {
                ApplicationUser user = JsonConvert.DeserializeObject<ApplicationUser>(response.ToString());

                return user;
            }

            return null;
        }

        public async Task<UsersResponseResult> GetListAsync(
            string token, int pageIndex, int pageSize, int orderType, string sortBy)
        {
            try
            {
                JObject response = await this.TokenHttpGetAsync(token,
                    $"api/users/list?PageIndex={pageIndex}&" +
                    $"PageSize={pageSize}&OrderType={orderType}&SortBy={sortBy}");

                if (response != null)
                {
                    return JsonConvert.DeserializeObject<UsersResponseResult>(response.ToString());
                    //IEnumerable<ApplicationUser> users = JsonConvert.DeserializeObject<
                    //    IEnumerable<ApplicationUser>>(
                    //    response.Value<string>("result").ToString());

                    //return users;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.GetListAsync exception", ex);
            }
        }

        public async Task<UsersResponseResult> GetListByPhoneNumberAsync(string token,
            string phonenumber, int pageIndex, int pageSize, int orderType, string sortBy)
        {
            try
            {
                JObject response = await this.TokenHttpGetAsync(token,
                    $"api/users/listbyphonenumber?PhoneNumber={phonenumber}" +
                    $"&PageIndex={pageIndex}&" +
                    $"PageSize={pageSize}&OrderType={orderType}&SortBy={sortBy}");

                if (response != null)
                {
                    return JsonConvert.DeserializeObject<UsersResponseResult>(response.ToString());
                    //IEnumerable<ApplicationUser> users = JsonConvert.DeserializeObject<
                    //    IEnumerable<ApplicationUser>>(
                    //    response.Value<string>("result").ToString());

                    //return users;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.GetListByPhoneNumberAsync exception", ex);
            }
        }

        public async Task<UserResponseResult> FindUserByLoginAsync(string loginProvider, string providerKey)
        {
            try
            {
                JObject response = await this.HttpGetAsync(
                    $"api/users/finduserbylogin?LoginProvider={loginProvider}" +
                           $"&ProviderKey={providerKey}");

                if (response != null)
                {
                    return JsonConvert.DeserializeObject<UserResponseResult>(response.ToString());
                    //ApplicationUser user = JsonConvert.DeserializeObject<ApplicationUser>(
                    //    response.ToString());

                    //return user;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.FindUserByLoginAsync exception", ex);
            }
        }

        public async Task<UsersResponseResult> GetListByUserNameAsync(string token,
            string username, int pageIndex, int pageSize, int orderType, string sortBy)
        {
            try
            {
                JObject response = await this.TokenHttpGetAsync(token,
                    $"api/users/listbyusername?UserName={username}" +
                    $"&PageIndex={pageIndex}&" +
                    $"PageSize={pageSize}&OrderType={orderType}&SortBy={sortBy}");

                if (response != null)
                {
                    return JsonConvert.DeserializeObject<UsersResponseResult>(response.ToString());
                    //IEnumerable<ApplicationUser> users = JsonConvert.DeserializeObject<
                    //    IEnumerable<ApplicationUser>>(
                    //    response.Value<string>("result").ToString());

                    //return users;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.GetListByUserNameAsync exception", ex);
            }
        }

        /// <summary>
        /// 重设密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        public async Task<ActionResponseResult> ResetPasswordAsync(string userId, string newPassword)
        {
            try
            {
                JObject kvRequest = new JObject();
                kvRequest.Add("Key", userId);
                kvRequest.Add("Value", newPassword);
                JObject response = await this.HttpPostAsync("api/users/resetpassword", kvRequest);

                if (response != null)// && response.ContainsKey("result"))
                {
                    return JsonConvert.DeserializeObject<ActionResponseResult>(response.ToString());
                    // return response.Value<string>("result");
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.ResetPasswordAsync exception", ex);
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        public async Task<ActionResponseResult> ChangePasswordAsync(string token, string userId,
            string oldPassword, string newPassword)
        {
            try
            {
                JObject kvRequest = new JObject();
                kvRequest.Add("UserId", userId);
                kvRequest.Add("OldPassword", oldPassword);
                kvRequest.Add("NewPassword", newPassword);
                JObject response = await this.TokenHttpPostAsync(token, "api/users/changepassword", kvRequest);

                if (response != null)// && response.ContainsKey("result"))
                {
                    return JsonConvert.DeserializeObject<ActionResponseResult>(response.ToString());
                    // return response.Value<string>("result");
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.ChangePasswordAsync exception", ex);
            }
        }

        /// <summary>
        /// 创建一个User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<UserResponseResult> CreateUserAsync(ApplicationUser user)
        {
            try
            {
                JObject response = await this.HttpPostAsync("api/users/adduser", JObject.FromObject(user));
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<UserResponseResult>(response.ToString());
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.CreateUserAsync exception", ex);
            }
        }

        /// <summary>
        /// 更新一个User的信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<UserResponseResult> UpdateUserAsync(string token, ApplicationUser user)
        {
            try
            {
                JObject response = await this.TokenHttpPostAsync(token,
                    "api/users/updateuser", JObject.FromObject(user));

                if (response != null)
                {
                    return JsonConvert.DeserializeObject<UserResponseResult>(response.ToString());
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.UpdateUserAsync exception", ex);
            }
        }

        /// <summary>
        /// 删除一个用户，包含删除所有的角色关联关系/Claims
        /// </summary>
        /// <param name="userId"></param>
        public async Task<ActionResponseResult> DeleteUserAsync(string token, string userId)
        {
            try
            {
                JObject response = await this.TokenHttpPostAsync(token,
                    "api/users/deleteuser", JObject.FromObject(
                    new ApplicationUser() { Id = userId }));
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<ActionResponseResult>(response.ToString());
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.DeleteUserAsync exception", ex);
            }
        }

        /// <summary>
        /// 添加一个用户的账号关联登录信息。
        /// 例如微信：userLogin.LoginProvider = '微信'；userLogin.ProviderKey = ${OpenID}
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public async Task<UserLoginResponseResult> AddUserLoginAsync(ApplicationUserLogin userLogin)
        {
            try
            {
                JObject response = await this.HttpPostAsync("api/userlogins/adduserlogin",
                    JObject.FromObject(userLogin));
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<UserLoginResponseResult>(response.ToString());
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.AddUserLoginAsync exception", ex);
            }
        }

        /// <summary>
        /// 去除一个用户的账号关联信息
        /// </summary>
        /// <param name="userLogin"></param>
        public async Task<ActionResponseResult> RemoveUserLoginAsync(string token, ApplicationUserLogin userLogin)
        {
            try
            {
                JObject response = await this.TokenHttpPostAsync(token, "api/userlogins/removeuserlogin",
                    JObject.FromObject(userLogin));

                if (response != null)
                {
                    return JsonConvert.DeserializeObject<ActionResponseResult>(
                        response.ToString());
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.RemoveUserLoginAsync exception", ex);
            }
        }

        /// <summary>
        /// 获取一个用户账号关联信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserLoginsResponseResult> GetLoginsByUserIdAsync(string token, string userId)
        {
            try
            {
                JObject response = await this.TokenHttpGetAsync(token,
                    $"api/userlogins/getloginsbyuserid?UserId={userId}");

                if (response != null)
                {
                    return JsonConvert.DeserializeObject<UserLoginsResponseResult>(
                        response.ToString());
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.GetLoginsByUserIdAsync exception", ex);
            }
        }

        /// <summary>
        /// 获取一个用户账号关联信息（判断是否存在）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns></returns>
        public async Task<UserLoginResponseResult> GetLoginByUserIdLoginTypeAsync(
            string userId, string loginProvider, string providerKey)
        {
            try
            {
                JObject response = await this.HttpGetAsync($"api/userlogins/getloginbyuseridlogintype" +
                    $"?UserId={userId}&LoginProvider ={loginProvider}&ProviderKey={providerKey}");

                if (response != null)
                {
                    return JsonConvert.DeserializeObject<UserLoginResponseResult>(
                        response.ToString());
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.AddUserRoleAsync exception", ex);
            }
        }

        /// <summary>
        /// 获取一个账号关联信息（判断是否存在）
        /// </summary> 
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns></returns>
        public async Task<UserLoginResponseResult> GetUserLoginByProviderKey(//string token,
            string loginProvider, string providerKey)
        {
            try
            {
                JObject response = await this.HttpGetAsync(//token,
                    $"api/userlogins/getuserloginbyproviderkey" +
                    $"?loginProvider={loginProvider}&providerKey={providerKey}");

                if (response != null)
                {
                    return JsonConvert.DeserializeObject<UserLoginResponseResult>(
                        response.ToString());
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.AddUserRoleAsync exception", ex);
            }
        }

        /// <summary>
        /// 添加一个用户到某个角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleName">角色名</param>
        /// <returns></returns>
        public async Task<UserRoleResponseResult> AddUserRoleAsync(string userId, string roleName)
        {
            try
            {
                JObject response = await this.HttpGetAsync("api/roles/findbyname?roleName=" + roleName);

                if (response != null)
                {
                    ApplicationRole role = JsonConvert.DeserializeObject<
                        RoleResponseResult>(response.ToString()).Data;

                    if (role != null)
                    {
                        return await this.AddUserRoleAsync(
                            new ApplicationUserRole()
                            {
                                UserId = userId,
                                RoleId = role.Id
                            }
                        );
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.AddUserRoleAsync exception", ex);
            }
        }

        /// <summary>
        /// 添加一个用户到某个角色
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns></returns>
        public async Task<UserRoleResponseResult> AddUserRoleAsync(ApplicationUserRole userRole)
        {
            try
            {
                var response = await this.HttpPostAsync("api/userroles/adduserrole", JObject.FromObject(userRole));
                if (response != null)
                    return JsonConvert.DeserializeObject<UserRoleResponseResult>(response.ToString());

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.AddUserRoleAsync exception", ex);
            }
        }

        /// <summary>
        /// 添加一个用户到多个角色
        /// </summary>
        /// <param name="userRoles"></param>
        /// <returns></returns>
        public async Task<UserRolesResponseResult> AddUserRolesAsync(
            IEnumerable<ApplicationUserRole> userRoles)
        {
            try
            {
                JArray array = new JArray();
                foreach (var role in userRoles)
                {
                    array.Add(JObject.FromObject(role));
                }
                var response = await this.HttpPostAsync("api/userroles/adduserroles", array);
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<UserRolesResponseResult>(
                        response.ToString());
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.AddUserRolesAsync exception", ex);
            }
        }

        /// <summary>
        /// 从某个角色中删除一个用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleName">角色名称</param>
        public async Task<ActionResponseResult> RemoveUserRoleAsync(string token, string userId, string roleName)
        {
            try
            {
                JObject response1 = await this.TokenHttpGetAsync(token,
                    $"api/roles/findbyname" +
                    $"?roleName={roleName}");

                if (response1 != null)
                {
                    ApplicationRole role = JsonConvert.DeserializeObject<RoleResponseResult>(
                        response1.ToString()).Data;

                    var response = await this.TokenHttpPostAsync(token, "api/userroles/removeuserrole",
                        JObject.FromObject(new ApplicationUserRole() { UserId = userId, RoleId = role.Id }));
                    if (response != null)
                    {
                        return JsonConvert.DeserializeObject<ActionResponseResult>(response.ToString());
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.RemoveUserRoleAsync exception", ex);
            }
        }

        /// <summary>
        /// 从某个角色中删除一个用户
        /// </summary> 
        /// <param name="userRole"></param>
        public async Task<ActionResponseResult> RemoveUserRoleAsync(string token, ApplicationUserRole userRole)
        {
            try
            {
                var response = await this.TokenHttpPostAsync(token, "api/userroles/removeuserrole",
                    JObject.FromObject(userRole));

                if (response != null)
                {
                    return JsonConvert.DeserializeObject<ActionResponseResult>(response.ToString());
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.RemoveUserRoleAsync exception", ex);
            }
        }

        /// <summary>
        /// 删除用户的多个角色关联
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userRoles"></param>
        /// <returns></returns>
        public async Task<ActionResponseResult> RemoveUserRolesAsync(string token, IEnumerable<ApplicationUserRole> userRoles)
        {
            try
            {
                var response = await this.TokenHttpPostAsync(token, "api/userroles/removeuserroles",
                    JObject.FromObject(userRoles));

                if (response != null)
                {
                    return JsonConvert.DeserializeObject<ActionResponseResult>(response.ToString());
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.RemoveUserRolesAsync exception", ex);
            }
        }

        /// <summary>
        /// 获取一个用户的所有角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<RolesResponseResult> GetRolesByUserIdAsync(string userId)
        {
            try
            {
                JObject response = await this.HttpGetAsync($"api/users/getrolesbyuserid?userId={userId}");

                if (response != null)
                {
                    return JsonConvert.DeserializeObject<
                        RolesResponseResult>(response.ToString());
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.GetRolesByUserIdAsync exception", ex);
            }
        }

        /// <summary>
        /// 获取一个用户所有的权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public async Task<UserClaimsResponseResult> GetClaimsByUserIdAsync(string userId)
        {
            try
            {
                JObject response = await this.HttpGetAsync($"api/users/getclaimsbyuserid?userId={userId}");

                if (response != null)
                {
                    return JsonConvert.DeserializeObject<UserClaimsResponseResult>(
                        response.ToString());
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.GetClaimsByUserIdAsync exception", ex);
            }
        }

        /// <summary>
        /// 锁定一个用户
        /// </summary>
        /// <param name="token">管理员Token</param>
        /// <param name="userId">用户ID</param>
        /// <param name="lockoutTimeUtc">锁定截止日期（用UTC时间），如果要永久锁定，设为100年后就够了</param>
        /// <returns></returns>
        public async Task<ActionResponseResult> LockUser(string token, string userId, DateTime lockoutTimeUtc)
        {
            try
            {
                LockoutUserRequest request = new LockoutUserRequest()
                {
                    LockoutEndTimeUtc = lockoutTimeUtc,
                    UserId = userId
                };
                var response = await this.TokenHttpPostAsync(token, "api/users/lockuser",
                    JObject.FromObject(request));
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<ActionResponseResult>(
                        response.ToString());
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.LockUser exception", ex);
            }
        }

        /// <summary>
        /// 解锁一个用户
        /// </summary>
        /// <param name="token">管理员Token</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public async Task<ActionResponseResult> UnlockUser(string token, string userId)
        {
            try
            {
                //JObject jobject = new JObject();
                //jobject.Add("userId", userId);
                var response = await this.TokenHttpPostAsync(token, "api/users/unlockuser",
                    JObject.FromObject(new ApplicationUser() { Id = userId }));

                if (response != null)
                {
                    return JsonConvert.DeserializeObject<ActionResponseResult>(
                        response.ToString());
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new AuthException("UserManager.UnlockUser exception", ex);
            }
        }
    }
}