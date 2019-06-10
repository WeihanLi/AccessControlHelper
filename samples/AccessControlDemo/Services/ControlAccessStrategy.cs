using Microsoft.AspNetCore.Http;
using WeihanLi.AspNetMvc.AccessControlHelper;

namespace AccessControlDemo.Services
{
    public class ControlAccessStrategy : IControlAccessStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ControlAccessStrategy(IHttpContextAccessor httpContextAccessor) =>
            _httpContextAccessor = httpContextAccessor;

        public bool IsControlCanAccess(string accessKey)
        {
            if (!string.IsNullOrWhiteSpace(accessKey) && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return true;
            }
            return false;
        }
    }
}
