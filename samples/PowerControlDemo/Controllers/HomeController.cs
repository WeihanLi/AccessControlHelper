using System;
using System.Linq;
using System.Web.Mvc;
using WeihanLi.AspNetMvc.AccessControlHelper;

namespace PowerControlDemo.Controllers
{
    [AllowAnonymous]
    [AccessControl]
    public class HomeController : Controller
    {
        [NoAccessControl]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AccessControl(AccessKey = "Contact")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AccessControl]
        public JsonResult Access(string accessKey)
        {
            return Json(string.IsNullOrEmpty(accessKey), JsonRequestBehavior.AllowGet);
        }
    }
}