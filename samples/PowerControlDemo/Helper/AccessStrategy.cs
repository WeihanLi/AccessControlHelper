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
            var user = HttpContext.Current.User.Identity.Name;
            var role = CommonHelper.GetUserRoleInfo(user);
            if (role.Any(r => r.RoleName.Contains("超级管理员")))
            {
                return true;
            }
            if (String.IsNullOrEmpty(accessKey))
            {
                if (role.Any(r => r.RoleName.StartsWith("门店")))
                {
                    return true;
                }
            }
            else
            {
                var accessList = Helper.CommonHelper.GetPowerList(HttpContext.Current.User.Identity.Name);
                if (accessList != null && accessList.Any(a => a.AccessKey == Guid.Parse(accessKey)))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class ActionAccessStrategy : IActionAccessStrategy
    {

        public bool IsActionCanAccess(string areaName, string controllerName, string actionName)
        {
            return false;
        }

        public ActionResult DisallowedCommonResult => new ContentResult()
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