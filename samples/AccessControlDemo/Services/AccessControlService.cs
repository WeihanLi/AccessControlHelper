using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace AccessControlDemo.Services
{
    public class AccessControlService
    {
        private static readonly Dictionary<string, string> _permissionDic = new Dictionary<string, string>()
        {
            { "/Home/About", "" },
            { "/Home/Contact", "Contact" },
            { "/Account/LogOut", "" }
        };

        public static bool IsCanAccess(PathString path, HttpContext context)
        {
            var matchedPath = _permissionDic.Keys.FirstOrDefault(p => path.StartsWithSegments(p));
            if (matchedPath != null)
            {
                var permission = _permissionDic[matchedPath];
                if (!context.User.Identity.IsAuthenticated)
                {
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(permission))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
