using System;
using System.Linq;
using System.Web.Mvc;
using AccessControlHelper;

namespace PowerControlDemo.Controllers
{
    [Authorize]
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

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateMenu()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMenu()
        {
            return View();
        }
    }
}