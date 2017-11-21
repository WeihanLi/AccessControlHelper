using System;
using System.Reflection;
using System.Linq;

#if NET45

using System.Web.Mvc;

#else

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;

#endif

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    /// <summary>
    /// 权限控制
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
#if NET45
    public class AccessControlAttribute : FilterAttribute, IAuthorizationFilter
#else
    public class AccessControlAttribute : Attribute, IAuthorizationFilter
#endif
    {
        public string AccessKey { get; set; }

        private static IActionAccessStrategy _accessStrategy;

        static AccessControlAttribute()
        {
            if (_accessStrategy == null)
            {
                _accessStrategy = WeihanLi.Common.DependencyResolver.Current.GetService<IActionAccessStrategy>();
                if (_accessStrategy == null)
                {
                    throw new ArgumentException("ActionResult显示策略未初始化，请使用注册显示策略", nameof(_accessStrategy));
                }
            }
        }

#if NET45
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException(nameof(filterContext));
            bool isDefined = filterContext.ActionDescriptor.IsDefined(typeof(NoAccessControlAttribute), true);
            if (!isDefined)
            {
                var area = filterContext.RouteData.Values["area"]?.ToString() ?? "";
                var controller = filterContext.RouteData.Values["controller"].ToString();
                var action = filterContext.RouteData.Values["action"].ToString();
                if (!_accessStrategy.IsActionCanAccess(area, controller, action, AccessKey))
                {
                    //if Ajax request
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = _accessStrategy.DisallowedAjaxResult;
                    }
                    else
                    {
                        filterContext.Result = _accessStrategy.DisallowedCommonResult;
                    }
                }
            }
        }
#else

        public virtual void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException(nameof(filterContext));
            bool isDefined = false;
            if (filterContext.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes()
                    .Any(a => a.GetType().Equals(typeof(NoAccessControlAttribute)));
            }
            if (!isDefined)
            {
                var area = filterContext.RouteData.Values["area"]?.ToString() ?? "";
                var controller = filterContext.RouteData.Values["controller"].ToString();
                var action = filterContext.RouteData.Values["action"].ToString();
                if (!_accessStrategy.IsActionCanAccess(area, controller, action, AccessKey))
                {
                    //if Ajax request
                    filterContext.Result = filterContext.HttpContext.Request.IsAjaxRequest() ?
                        _accessStrategy.DisallowedAjaxResult :
                        _accessStrategy.DisallowedCommonResult;
                }
            }
        }

#endif
    }

#if !NET45

    public static class AjaxRequestExtensions
    {
        /// <summary>
        /// 判断是否是Ajax请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsAjaxRequest(this Microsoft.AspNetCore.Http.HttpRequest request)
        {
            return request?.Headers != null && String.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);
        }
    }

#endif
}