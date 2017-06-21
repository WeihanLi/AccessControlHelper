namespace PowerControlDemo.Models
{
    public class ShopUserRoleModel : BaseModel
    {
        private string roleName;

        public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }

        private string roleDesc;

        public string RoleDesc
        {
            get { return roleDesc; }
            set { roleDesc = value; }
        }
    }
}