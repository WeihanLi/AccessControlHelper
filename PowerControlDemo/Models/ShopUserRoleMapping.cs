namespace PowerControlDemo.Models
{
    public class ShopUserRoleMappingModel : BaseModel
    {
        private int? userId;

        public int? UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private int roleId;

        public int RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private int mappingRule;

        public int MappingRule
        {
            get { return mappingRule; }
            set { mappingRule = value; }
        }
    }
}