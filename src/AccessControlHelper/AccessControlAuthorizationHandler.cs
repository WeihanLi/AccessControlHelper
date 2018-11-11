using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    public class AccessControlAuthorizationHandler : AuthorizationHandler<AccessControlRequirement>
    {
        private readonly IResourceAccessStrategy _resourceAccessStrategy;
        private readonly string _accessKey = string.Empty;

        public AccessControlAuthorizationHandler(IResourceAccessStrategy resourceAccessStrategy)
        {
            _resourceAccessStrategy = resourceAccessStrategy;
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
