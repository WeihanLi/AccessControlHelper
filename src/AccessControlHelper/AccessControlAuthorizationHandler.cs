using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    internal sealed class AccessControlAuthorizationHandler : AuthorizationHandler<AccessControlRequirement>
    {
        private readonly AccessControlOptions _options;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccessControlAuthorizationHandler(IHttpContextAccessor contextAccessor, IOptions<AccessControlOptions> options)
        {
            _contextAccessor = contextAccessor;
            _options = options.Value;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessControlRequirement requirement)
        {
            var httpContext = _contextAccessor.HttpContext;
            var accessKey = _options.AccessKeyResolver?.Invoke(httpContext);
            var resourceAccessStrategy = httpContext.RequestServices.GetService<IResourceAccessStrategy>();
            if (resourceAccessStrategy.IsCanAccess(accessKey))
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
