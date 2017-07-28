using System.Web.Mvc;

namespace AccessControlHelper
{
    /// <summary>
    /// ActionResult显示策略
    /// </summary>
    public interface IActionResultDisplayStrategy
    {
        /// <summary>
        /// 是否可以显示
        /// </summary>
        /// <param name="areaName">区域名称</param>
        /// <param name="controllerName">控制器名称</param>
        /// <param name="actionName">action名称</param>
        /// <returns></returns>
        bool IsActionCanAccess(string areaName,string controllerName,string actionName);

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