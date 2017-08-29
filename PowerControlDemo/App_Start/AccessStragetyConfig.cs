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
            WeihanLi.AspNetMvc.AccessControlHelper.HtmlHelperExtension.RegisterAccessStrategy(new Helper.ControlAccessStrategy());
            //RegisterActionResultDisplayStrategy
            WeihanLi.AspNetMvc.AccessControlHelper.AccessControlAttribute.RegisterAccessStrategy(new Helper.ActionAccessStrategy());
        }
    }
}