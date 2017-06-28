using PowerControlDemo.Helper;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PowerControlDemo.Filter
{
    /// <summary>
    /// 权限控制
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,AllowMultiple =true,Inherited =true)]
    public class AccessControlAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var roleInfo = CommonHelper.GetUserRoleInfo(HttpContext.Current.User.Identity.Name);
            if (roleInfo.Any(r => r.RoleName.Contains("超级管理员")))
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                var area = filterContext.RouteData.Values["area"]?.ToString().ToLower() ?? "";
                var controller = filterContext.RouteData.Values["controller"].ToString().ToLower();
                var action = filterContext.RouteData.Values["action"].ToString().ToLower();

                var accessKey = CommonHelper.BusinessHelper.ShopAccessConfigHelper.Fetch(a => a.AreaName.ToLower() == area && a.ControllerName.ToLower() == controller && a.ActionName.ToLower() == action && a.ControlType == 0 && !a.IsDeleted);
                var accessList = CommonHelper.GetPowerList(HttpContext.Current.User.Identity.Name);
                if (accessList != null && accessList.Any(a => a.PKID == accessKey.PKID))
                {
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    ActionResult result = new ContentResult
                    {
                        Content = "<h3 style=\"color:red;\">You do not have the key to the entrance.</h3>",
                        ContentEncoding = System.Text.Encoding.UTF8,
                        ContentType = "text/html"
                    };
                    //if Ajax request
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        result = new JsonResult
                        {
                            Data = "You do not have the key to the entrance.",
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    filterContext.Result = result;
                }
            }
        }
    }
}