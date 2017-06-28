using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerControlDemo.Helper
{
    public class CommonHelper
    {
        private static Business.IBusinessHelper businessHelper;
        private static object locker = new object();
        public static readonly string accessConfigCachePrefix = "accessKeyConfig#";
        public static readonly string roleConfigCachePrefix = "roleConfig#";
        /// <summary>
        /// BusinessHelper
        /// </summary>
        public static Business.IBusinessHelper BusinessHelper
        {
            get
            {
                if (businessHelper == null)
                {
                    lock (locker)
                    {
                        if (businessHelper == null)
                        {
                            businessHelper = new Business.BusinessHelper();
                        }
                    }
                }
                return businessHelper;
            }
        }

        /// <summary>
        /// 根据登录用户获取用户权限
        /// </summary>
        /// <param name="userName">登录用户用户名</param>
        /// <returns></returns>
        public static List<Models.ShopAccessConfigModel> GetPowerList(string userName)
        {
            var accessConfig = RedisHelper.Get<List<Models.ShopAccessConfigModel>>(accessConfigCachePrefix+userName);
            if (accessConfig == null)
            {
                var user = BusinessHelper.ShopUserHelper.Fetch(u => !u.IsDeleted && (u.Email == userName || u.Mobile == userName || u.UserName == userName));
                if (user != null)
                {
                    var accessList = new List<long>();
                    // 此处权限列表应从缓存中获取，从缓存中获取不到再查询数据库
                    var roleInfoList = GetUserRoleInfo(userName);
                    if (roleInfoList.Any(r=>r.RoleName.Contains("超级管理员")))
                    {
                        //超级管理员拥有所有权限
                        accessConfig = BusinessHelper.ShopAccessConfigHelper.GetAll(c => !c.IsDeleted);
                        return accessConfig;
                    }
                    roleInfoList = roleInfoList.Where(r => r != null).ToList();
                    foreach (var roleInfo in roleInfoList)
                    {
                        var roleAccess = BusinessHelper.ShopUserRoleAccessHelper.GetAll(r => r.RoleId == roleInfo.PKID && !r.IsDeleted).Select(s => s.AccessId);
                        if (roleAccess != null && roleAccess.Any())
                        {
                            accessList.AddRange(roleAccess);
                        }
                    }
                    // user access
                    var userAccess = BusinessHelper.ShopUserAccessHelper.GetAll(s => s.UserId == user.UserGuid && !s.IsDeleted).Select(s => s.AccessId);
                    if (userAccess != null && userAccess.Any())
                    {
                        accessList.AddRange(userAccess);
                    }
                    var accessIds = accessList.Distinct().ToList();
                    accessConfig = BusinessHelper.ShopAccessConfigHelper.GetAll(c => accessIds.Contains(c.PKID) && !c.IsDeleted);
                    if (accessConfig != null || accessConfig.Any())
                    {
                        RedisHelper.Set(accessConfigCachePrefix + userName, accessConfig,TimeSpan.FromDays(1));
                    }
                    else
                    {
                        RedisHelper.Set(accessConfigCachePrefix + userName, new List<Models.ShopAccessConfigModel>(), TimeSpan.FromDays(1));
                    }
                    return accessConfig;
                }
                return null;
            }
            return accessConfig;
        }

        /// <summary>
        /// 获取用户角色信息
        /// </summary>
        /// <param name="userName">用户信息</param>
        /// <returns></returns>
        public static List<Models.ShopUserRoleModel> GetUserRoleInfo(string userName)
        {
            //先从redis中获取
            var roleInfo = RedisHelper.Get<List<Models.ShopUserRoleModel>>(roleConfigCachePrefix + userName);
            if (roleInfo == null)
            {
                var user = BusinessHelper.ShopUserHelper.Fetch(u => !u.IsDeleted && (u.Email == userName || u.UserName == userName || u.Mobile == userName));
                if (user != null)
                {
                    var mapping = BusinessHelper.ShopUserRoleMappingHelper.Fetch(s => !s.IsDeleted && ((s.UserId == user.UserGuid && s.MappingRule == 0) || (user.Email.StartsWith(s.UserName) && s.MappingRule == 1)));
                    if (mapping != null)
                    {
                        var roleIds = mapping.RoleId.Split(',').Select(s => Convert.ToInt32(s)).ToList();
                        roleInfo = BusinessHelper.ShopUserRoleHelper.GetAll(r => roleIds.Contains(r.PKID) && !r.IsDeleted);
                        RedisHelper.Set(roleConfigCachePrefix + userName, roleInfo, TimeSpan.FromDays(1));
                    }
                }
            }
            return roleInfo;
        }

        /// <summary>
        /// 判断用户是不是超级管理员
        /// </summary>
        /// <param name="userName">用户信息</param>
        /// <returns>true是，false不是</returns>
        public bool IsUserSupperAdmin(string userName)
        {
            return GetUserRoleInfo(userName).Any(s => s.RoleName.Contains("超级管理员"));
        }
    }
}