using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    public class AccessControlAuthorizationHandler : AuthorizationHandler<AccessControlRequirement>
    {
        private readonly IResourceAccessStrategy _resourceAccessStrategy;
        private readonly string _accessKey = string.Empty;
        private readonly AccessControlOptions _option;

        public AccessControlAuthorizationHandler(IResourceAccessStrategy resourceAccessStrategy, IHttpContextAccessor contextAccessor, IOptions<AccessControlOptions> options)
        {
            _resourceAccessStrategy = resourceAccessStrategy;
            _option = options.Value;
            if (contextAccessor.HttpContext.Request.Headers.ContainsKey(_option.AccessHeaderKey))
            {
                _accessKey = contextAccessor.HttpContext.Request.Headers[_option.AccessHeaderKey].ToString();
            }
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessControlRequirement requirement)
        {
            if (_resourceAccessStrategy.IsCanAccess(_accessKey))
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
