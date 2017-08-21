using Microsoft.AspNetCore.Mvc;

namespace AccessControlDemo.Services
{
    public class AccessActionDisplayStrategy : AccessControlHelper.IActionDisplayStrategy
    {
        public bool IsActionCanAccess(string areaName, string controllerName, string actionName)
        {
            return false;
        }

        public IActionResult DisallowedCommonResult => new ContentResult { Content = "You have no access", ContentType = "text/html", StatusCode = 401 };

        public JsonResult DisallowedAjaxResult => new JsonResult(new { Data = "You have no access", Code = 401 });
    }
}