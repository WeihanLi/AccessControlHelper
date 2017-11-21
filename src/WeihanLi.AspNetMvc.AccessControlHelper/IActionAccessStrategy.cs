#if NET45
using System.Web.Mvc;
#else

using Microsoft.AspNetCore.Mvc;

#endif

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    /// <summary>
    /// Action显示策略
    /// </summary>
    public interface IActionAccessStrategy
    {
        /// <summary>
        /// 是否可以显示
        /// </summary>
        /// <param name="areaName">区域名称</param>
        /// <param name="controllerName">控制器名称</param>
        /// <param name="actionName">action名称</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        bool IsActionCanAccess(string areaName, string controllerName, string actionName, string accessKey);

        /// <summary>
        /// 默认HTTP请求不被授权时返回的结果
        /// </summary>
#if NET45
        ActionResult DisallowedCommonResult { get; }
#else
        IActionResult DisallowedCommonResult { get; }
#endif

        /// <summary>
        /// Ajax请求不被授权时返回的结果
        /// </summary>
        JsonResult DisallowedAjaxResult { get; }
    }
}