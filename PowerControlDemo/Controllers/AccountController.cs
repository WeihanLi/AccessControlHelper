using System.Web.Mvc;
using System.Web.Security;
using WeihanLi.Common.Helpers;

namespace PowerControlDemo.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Logon(ViewModels.LogonViewModel model)
        {
            bool result = false;
            var user = Helper.CommonHelper.BusinessHelper.ShopUserHelper.Fetch(s => s.IsDeleted == false && s.UserName == model.UserName);
            if (user != null && user.PasswordHash.Equals(HashHelper.GetHashedString(HashType.SHA256, model.Password)))
            {
                result = true;
                FormsAuthentication.SetAuthCookie(user.UserName, model.RememberMe);
                //请求权限数据
                var userPower = Helper.CommonHelper.GetPowerList(user.UserName);
            }
            return Json(result);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            Helper.RedisHelper.Remove(Helper.CommonHelper.accessConfigCachePrefix + User.Identity.Name);
            Helper.RedisHelper.Remove(Helper.CommonHelper.roleConfigCachePrefix + User.Identity.Name);
            FormsAuthentication.SignOut();
            return Redirect("Login");
        }
    }
}