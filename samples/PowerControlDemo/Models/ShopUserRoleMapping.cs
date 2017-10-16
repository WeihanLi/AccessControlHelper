using WeihanLi.EntityFramework.DbDescriptionHelper;
using System;

namespace PowerControlDemo.Models
{
    [TableDescription("ShopUserRoleMapping", "用户角色映射表")]
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

        private int userType;

        [ColumnDescription("用户类型")]
        public int UserType
        {
            get { return userType; }
            set { userType = value; }
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