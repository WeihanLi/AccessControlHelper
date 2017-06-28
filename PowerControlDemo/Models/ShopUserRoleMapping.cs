using System;

namespace PowerControlDemo.Models
{
    public class ShopUserRoleMappingModel : BaseModel
    {
        private Guid? userId;
        [ColumnDescription("用户Guid")]
        public Guid? UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private string roleId;
        [ColumnDescription("角色id")]
        public string RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }

        private string userName;
        [ColumnDescription("用户名")]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private int mappingRule;
        [ColumnDescription("用户角色映射策略")]
        public int MappingRule
        {
            get { return mappingRule; }
            set { mappingRule = value; }
        }
    }
}