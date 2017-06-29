using System;
using System.Linq;
using System.Web.Mvc;
using AccessControlHelper;

namespace PowerControlDemo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AccessControl]
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