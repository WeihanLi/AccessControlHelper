using PowerControlDemo.Helper;
using WeihanLi.AspNetMvc.AccessControlHelper;

namespace PowerControlDemo
{
    /// <summary>
    /// 权限控制显示策略配置
    /// </summary>
    public static class AccessStragetyConfig
    {
        /// <summary>
        /// 注册显示策略
        /// </summary>
        public static void RegisterDisplayStrategy()
        {
            // RegisterControlDisplayStrategy
            AccessControlHelperExtensions.RegisterAccessStragety(new AccessControlHelperOptions
            {
                ActionAccessStrategy = new ActionAccessStrategy(),
                ControlAccessStrategy = new ControlAccessStrategy()
            });
        }
    }
}