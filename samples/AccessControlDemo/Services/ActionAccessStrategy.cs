using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using WeihanLi.AspNetMvc.AccessControlHelper;

namespace AccessControlDemo.Services
{
    public class ActionAccessStrategy : IResourceAccessStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActionAccessStrategy(IHttpContextAccessor httpContextAccessor) =>
            _httpContextAccessor = httpContextAccessor;

        public bool IsCanAccess(string accessKey)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var features = httpContext.Features;

            var area = httpContext.GetRouteValue("area");
            var controller = httpContext.GetRouteValue("controller");
            var action = httpContext.GetRouteValue("action");

            var routeData = httpContext.GetRouteData();
            return AccessControlService.IsCanAccess(httpContext.Request.Path, httpContext);
        }

        public string StrategyName { get; } = "Global";

        public IActionResult DisallowedCommonResult => new ContentResult
        {
            Content = "You have no access",
            ContentType = "text/html",
            StatusCode = 403
        };

        public IActionResult DisallowedAjaxResult => new JsonResult(new { Data = "You have no access", Code = 403 });
    }
}
