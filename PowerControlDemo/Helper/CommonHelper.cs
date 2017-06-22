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
                    var roleInfo = BusinessHelper.ShopUserRoleMappingHelper.Fetch(s => !s.IsDeleted && ((s.UserId == user.UserGuid && s.MappingRule == 0) || (user.Email.StartsWith(s.UserName) && s.MappingRule == 1)));
                    if (roleInfo != null)
                    {
                        var roleAccess = BusinessHelper.ShopUserRoleAccessHelper.GetAll(r => r.RoleId == roleInfo.PKID && !r.IsDeleted).Select(s => s.AccessId);
                        if (roleAccess != null)
                        {
                            accessList.AddRange(roleAccess);
                        }
                    }
                    //
                    var userAccess = BusinessHelper.ShopUserAccessHelper.GetAll(s => s.UserId == user.UserGuid && !s.IsDeleted).Select(s => s.AccessId);
                    if (userAccess != null)
                    {
                        //
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
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        public static Models.ShopUserRoleModel GetUserRoleInfo(string userName)
        {
            var user = BusinessHelper.ShopUserHelper.Fetch(u => !u.IsDeleted && (u.Email == userName || u.UserName == userName || u.Mobile == userName));
            if(user!=null)
            {
                var mapping = BusinessHelper.ShopUserRoleMappingHelper.Fetch(s => !s.IsDeleted && ((s.UserId == user.UserGuid && s.MappingRule == 0) || (user.Email.StartsWith(s.UserName) && s.MappingRule == 1)));
                if (mapping != null)
                {
                    return BusinessHelper.ShopUserRoleHelper.Fetch(r => r.PKID == mapping.RoleId && !r.IsDeleted);
                }
            }
            return null;
        }
    }
}