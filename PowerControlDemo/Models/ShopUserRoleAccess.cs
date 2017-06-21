namespace PowerControlDemo.Models
{
    public class ShopUserRoleAccessModel : BaseModel
    {
        private int roleId;

        public int RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }

        private long accessId;

        public long AccessId
        {
            get { return accessId; }
            set { accessId = value; }
        }
    }
}