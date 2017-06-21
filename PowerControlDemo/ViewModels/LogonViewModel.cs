namespace PowerControlDemo.ViewModels
{
    /// <summary>
    /// 登录model
    /// </summary>
    public class LogonViewModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// RememberMe
        /// </summary>
        public bool RememberMe { get; set; }
    }
}