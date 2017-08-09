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
        private static IActionResultDisplayStrategy _displayStrategy;

        public static void RegisterDisplayStrategy<TStrategy>(TStrategy strategy) where TStrategy : IActionResultDisplayStrategy
        {
            _displayStrategy = strategy;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
#if NET45
            if (!filterContext.ActionDescriptor.IsDefined(typeof(NoAccessControlAttribute), true))
#else
            bool isDefined = false;
            var controllerActionDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                    .Any(a => a.GetType().Equals(typeof(NoAccessControlAttribute)));
            }
            if(!isDefined)
#endif
            {
                if (_displayStrategy == null)
                {
                    throw new ArgumentException("ActionResult显示策略未初始化，请使用 AccessControlAttribute.RegisterDisplayStrategy(IActionResultDisplayStrategy stragety) 方法注册显示策略", nameof(_displayStrategy));
                }
                var area = filterContext.RouteData.Values["area"]?.ToString() ?? "";
                var controller = filterContext.RouteData.Values["controller"].ToString();
                var action = filterContext.RouteData.Values["action"].ToString();
                if (_displayStrategy.IsActionCanAccess(area, controller, action))
                {
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    //if Ajax request
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = _displayStrategy.DisallowedAjaxResult;
                    }
                    else
                    {
                        filterContext.Result = _displayStrategy.DisallowedCommonResult;
                    }
                }
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }
    }
#if NETSTANDARD1_6
    public static class AjaxRequestExtensions
    {
        /// <summary>
        /// 判断是否是Ajax请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsAjaxRequest(this Microsoft.AspNetCore.Http.HttpRequest request)
        {
            return (request != null && request.Headers != null && (request.Headers["X-Requested-With"] == "XMLHttpRequest"));
        }
    }
#endif
}