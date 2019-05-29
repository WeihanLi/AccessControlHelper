using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeihanLi.AspNetMvc.AccessControlHelper;

namespace AccessControlDemo.Controllers
{
    //[AccessControl]
    public class HomeController : Controller
    {
        [NoAccessControl]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        //[AccessControl]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [AccessControl(AccessKey = "Contact")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
