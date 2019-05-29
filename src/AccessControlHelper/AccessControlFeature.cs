namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    public interface IAccessControlFeature
    {
        bool NoAccessControl { get; }
    }

    public class AccessControlFeature : IAccessControlFeature
    {
        public bool NoAccessControl { get; set; }
    }
}
