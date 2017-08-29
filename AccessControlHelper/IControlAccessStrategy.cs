namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    /// <summary>
    /// Control显示策略
    /// </summary>
    public interface IControlAccessStrategy
    {
        /// <summary>
        /// 是否可以显示
        /// </summary>
        bool IsControlCanAccess(string accessKey);
    }
}