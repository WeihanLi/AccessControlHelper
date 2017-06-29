namespace AccessControlHelper
{
    /// <summary>
    /// Control显示策略
    /// </summary>
    public interface IControlDisplayStrategy
    {
        /// <summary>
        /// 是否可以显示
        /// </summary>
        bool IsCanDisplay { get; }

        /// <summary>
        /// 隐藏的元素class名称
        /// </summary>
        string HiddenClassName { get; }

        /// <summary>
        /// AccessKey
        /// </summary>
        string AccessKey { get; set; }
    }
}