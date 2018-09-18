using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    public class AccessControlAuthorizationHandler : AuthorizationHandler<AccessControlRequirement>
    {
        private readonly IActionAccessStrategy _actionAccessStrategy;
        private readonly string _accessKey = string.Empty;

        public AccessControlAuthorizationHandler(IHttpContextAccessor contextAccessor, IActionAccessStrategy actionAccessStrategy)
        {
            _actionAccessStrategy = actionAccessStrategy;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessControlRequirement requirement)
        {
            if (_actionAccessStrategy.IsActionCanAccess(_accessKey))
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