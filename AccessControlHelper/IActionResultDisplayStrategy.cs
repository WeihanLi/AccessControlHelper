using System.Web.Mvc;

namespace AccessControlHelper
{
    /// <summary>
    /// ActionResult显示策略
    /// </summary>
    public interface IActionResultDisplayStrategy
    {
        /// <summary>
        /// AreaName
        /// </summary>
        string AreaName { get; set; }

        /// <summary>
        /// ControllerName
        /// </summary>
        string ControllerName { get; set; }

        /// <summary>
        /// ActionName
        /// </summary>
        string ActionName { get; set; }

        /// <summary>
        /// 是否可以显示
        /// </summary>
        bool IsCanDisplay { get;}

        /// <summary>
        /// 默认HTTP请求不被授权时返回的结果
        /// </summary>
        ActionResult DisallowedCommonResult { get; }

        /// <summary>
        /// Ajax请求不被授权时返回的结果
        /// </summary>
        JsonResult DisallowedAjaxResult { get; }
    }
}