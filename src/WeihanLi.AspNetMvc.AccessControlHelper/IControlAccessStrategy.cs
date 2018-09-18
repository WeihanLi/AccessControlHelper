namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    /// <summary>
    /// view componment access strategy
    /// </summary>
    public interface IControlAccessStrategy
    {
        /// <summary>
        /// view componment access strategy
        /// </summary>
        bool IsControlCanAccess(string accessKey);
    }
}