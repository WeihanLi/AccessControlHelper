using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeihanLi.AspNetMvc.AccessControlHelper;

namespace AccessControlDemo.Services
{
    public class ActionAccessStrategy : IActionAccessStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActionAccessStrategy(IHttpContextAccessor httpContextAccessor) =>
            _httpContextAccessor = httpContextAccessor;

        public bool IsActionCanAccess(string areaName, string controllerName, string actionName, string accessKey)
        => string.IsNullOrEmpty(accessKey) && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

        public IActionResult DisallowedCommonResult => new ContentResult { Content = "You have no access", ContentType = "text/html", StatusCode = 401 };

        public JsonResult DisallowedAjaxResult => new JsonResult(new { Data = "You have no access", Code = 401 });
    }
}