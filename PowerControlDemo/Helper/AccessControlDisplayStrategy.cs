using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccessControlHelper;

namespace PowerControlDemo.Helper
{
    public class AccessControlDisplayStrategy : IControlDisplayStrategy
    {
        /// <summary>
        /// HiddenClassName
        /// </summary>
        public string HiddenClassName => "tuhu-hidden";

        /// <summary>
        /// AccessKey
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// 是否可以显示
        /// </summary>
        public bool IsCanDisplay => IsControlCanDisplay();

        /// <summary>
        /// 判断是否可以显示
        /// </summary>
        /// <returns></returns>
        private bool IsControlCanDisplay()
        {
            var user = HttpContext.Current.User.Identity.Name;
            var role = CommonHelper.GetUserRoleInfo(user);
            if (role.Any(r => r.RoleName.Contains("超级管理员")))
            {
                return true;
            }
            if (String.IsNullOrEmpty(AccessKey))
            {
                if (role.Any(r => r.RoleName.StartsWith("门店")))
                {
                    return true;
                }
            }
            else
            {
                var accessList = Helper.CommonHelper.GetPowerList(HttpContext.Current.User.Identity.Name);
                if (accessList != null && accessList.Any(a => a.AccessKey == Guid.Parse(AccessKey)))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class AccessActionResultDisplayStrategy : IActionResultDisplayStrategy
    {
        public string AreaName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool IsCanDisplay => IsActionResultCanDisplay();

        private bool IsActionResultCanDisplay()
        {
            return false;
        }
        public ActionResult DisallowedCommonResult => new ContentResult()
        {
                
        };

        public JsonResult DisallowedAjaxResult => new JsonResult()
        {
            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            Data = ""
        };
    }
}