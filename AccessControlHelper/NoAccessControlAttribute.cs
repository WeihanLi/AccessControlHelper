using System;

#if NET45
using System.Web.Mvc;
#else

using Microsoft.AspNetCore.Mvc.Filters;

#endif

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