using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;
using WeihanLi.AspNetMvc.AccessControlHelper;

namespace AccessControlDemo.Controllers
{
    public class AccountController : Controller
    {
        [ActionName("Login")]
        public async Task<IActionResult> LoginAsync([FromServices]ILogger<AccountController> logger)
        {
            logger.LogDebug("login ing...");
            var u = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "testuser") }, CookieAuthenticationDefaults.AuthenticationScheme));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, u, new AuthenticationProperties { IsPersistent = true, AllowRefresh = true });
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(AccessControlHelperConstants.PolicyName)]
        public string Index() => "test";
    }
}
