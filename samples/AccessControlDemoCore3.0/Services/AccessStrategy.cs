using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeihanLi.AspNetMvc.AccessControlHelper;

namespace AccessControlDemoCore3._0.Services
{
    public class ResourceAccessStrategy : IResourceAccessStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResourceAccessStrategy(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsCanAccess(string accessKey)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            return httpContext.User.Identity.IsAuthenticated;
        }

        public IActionResult DisallowedCommonResult => new ContentResult
        {
            Content = "You have no access",
            ContentType = "text/html",
            StatusCode = 403
        };

        public IActionResult DisallowedAjaxResult => new JsonResult(new { Data = "You have no access", Code = 403 });
    }

    public class ControlAccessStrategy : IControlAccessStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ControlAccessStrategy(IHttpContextAccessor httpContextAccessor) =>
            _httpContextAccessor = httpContextAccessor;

        public bool IsControlCanAccess(string accessKey)
        {
            if ("Never".Equals(accessKey, System.StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
