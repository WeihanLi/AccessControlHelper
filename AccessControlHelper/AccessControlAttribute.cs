using System;
using System.Reflection;
using System.Linq;

#if NET45
using System.Web.Mvc;
#else

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;

#endif

namespace AccessControlHelper
{
    /// <summary>
    /// 权限控制
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AccessControlAttribute : ActionFilterAttribute
    {
        private static IActionAccessStrategy _accessStrategy;

        public static void RegisterAccessStrategy<TStrategy>(TStrategy strategy) where TStrategy : IActionAccessStrategy
        {
            _accessStrategy = strategy;
        }

        public AccessControlAttribute()
        {
        }

        public AccessControlAttribute(IActionAccessStrategy displayStrategy)
        {
            _accessStrategy = displayStrategy;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool isDefined = false;
#if NET45
            isDefined = filterContext.ActionDescriptor.IsDefined(typeof(NoAccessControlAttribute), true);
#else
            var controllerActionDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes()
                    .Any(a => a.GetType().Equals(typeof(NoAccessControlAttribute)));
            }
#endif
            if (!isDefined)
            {
                if (_accessStrategy == null)
                {
                    throw new ArgumentException("ActionResult显示策略未初始化，请使用 AccessControlAttribute.RegisterDisplayStrategy(IActionResultDisplayStrategy stragety) 方法注册显示策略", nameof(_accessStrategy));
                }
                var area = filterContext.RouteData.Values["area"]?.ToString() ?? "";
                var controller = filterContext.RouteData.Values["controller"].ToString();
                var action = filterContext.RouteData.Values["action"].ToString();
                if (_accessStrategy.IsActionCanAccess(area, controller, action))
                {
                    base.OnActionExecuting(filterContext);
                }
                else
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
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }
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