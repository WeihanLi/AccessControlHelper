using System;
using System.Web.Mvc;

namespace AccessControlHelper
{
    /// <summary>
    /// 不用权限控制
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoAccessControlAttribute : ActionFilterAttribute
    {
    }
}