using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    public class AccessControlRequirement : IAuthorizationRequirement
    {
        public AccessControlRequirement()
        {
        }
    }

    public class AccessControlHandler : AuthorizationHandler<AccessControlRequirement>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IActionAccessStrategy _actionAccessStrategy;

        public AccessControlHandler(IHttpContextAccessor contextAccessor, IActionAccessStrategy actionAccessStrategy)
        {
            _contextAccessor = contextAccessor;
            _actionAccessStrategy = actionAccessStrategy;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessControlRequirement requirement)
        {
            if (_actionAccessStrategy.IsActionCanAccess(_contextAccessor.HttpContext, ""))
            {
                context.Succeed(requirement);
            }
            context.Fail();
            return Task.CompletedTask;
        }
    }
}