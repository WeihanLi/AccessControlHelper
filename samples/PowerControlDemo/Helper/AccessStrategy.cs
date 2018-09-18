using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WeihanLi.AspNetMvc.AccessControlHelper;

namespace PowerControlDemo.Helper
{
    public class ControlAccessStrategy : IControlAccessStrategy
    {
        public bool IsControlCanAccess(string accessKey)
        {
            return accessKey == String.Empty;
        }
    }

    public class ActionAccessStrategy : IActionAccessStrategy
    {
        public bool IsActionCanAccess(string accessKey)
        {
            var isValid = string.IsNullOrEmpty(accessKey);

            var context = HttpContext.Current;
            var area = context.Request.RequestContext.RouteData.Values["area"];
            var controller = context.Request.RequestContext.RouteData.Values["controller"];
            var action = context.Request.RequestContext.RouteData.Values["action"];

            return isValid;
        }

        public ActionResult DisallowedCommonResult => new ContentResult
        {
            Content = "<h3>You have no permission!</h3>",
            ContentEncoding = Encoding.UTF8,
            ContentType = "text/html"
        };

        public JsonResult DisallowedAjaxResult => new JsonResult()
        {
            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            Data = "You have no permission!"
        };
    }
}