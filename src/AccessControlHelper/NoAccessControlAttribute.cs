using System;

#if NET45
using System.Web.Mvc;
#else

using Microsoft.AspNetCore.Mvc.Filters;

#endif

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    /// <summary>
    /// 不用权限控制
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
#if NET45
    public class NoAccessControlAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
        }
    }
#else
    public class NoAccessControlAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
        }
    }

#endif
}