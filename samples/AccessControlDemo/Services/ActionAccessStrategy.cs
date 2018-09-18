using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using WeihanLi.AspNetMvc.AccessControlHelper;

namespace AccessControlDemo.Services
{
    public class ActionAccessStrategy : IActionAccessStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActionAccessStrategy(IHttpContextAccessor httpContextAccessor) =>
            _httpContextAccessor = httpContextAccessor;

        public bool IsActionCanAccess(HttpContext httpContext, string accessKey)
        {
            var isValid = string.IsNullOrEmpty(accessKey) && httpContext.User.Identity.IsAuthenticated;

            var area = httpContext.GetRouteValue("area");
            var controller = httpContext.GetRouteValue("controller");
            var action = httpContext.GetRouteValue("action");

            return isValid;
        }

        public IActionResult DisallowedCommonResult => new ContentResult { Content = "You have no access", ContentType = "text/html", StatusCode = 403 };

        public JsonResult DisallowedAjaxResult => new JsonResult(new { Data = "You have no access", Code = 403 });
    }
}