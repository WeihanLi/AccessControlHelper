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

        [NoAccessControl]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult Access(string accessKey)
        {
            var result = false;
            if (!String.IsNullOrEmpty(accessKey))
            {
                var powerList = Helper.CommonHelper.GetPowerList(User.Identity.Name);
                if (powerList != null && powerList.Any(s => s.AccessKey == Guid.Parse(accessKey)))
                {
                    result = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}