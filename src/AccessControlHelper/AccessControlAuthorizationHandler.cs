using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    internal class AccessControlAuthorizationHandler : AuthorizationHandler<AccessControlRequirement>
    {
        private readonly string _accessKeyHeaderName;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccessControlAuthorizationHandler(IHttpContextAccessor contextAccessor, IOptions<AccessControlOptions> options)
        {
            _contextAccessor = contextAccessor;
            _accessKeyHeaderName = options.Value.AccessKeyHeaderName;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessControlRequirement requirement)
        {
            var httpContext = _contextAccessor.HttpContext;
            var resourceAccessStrategy = httpContext.RequestServices.GetService<IResourceAccessStrategy>();
            if (resourceAccessStrategy.IsCanAccess(httpContext.Request.Headers.TryGetValue(_accessKeyHeaderName, out var accessKey) ? accessKey.ToString() : ""))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
