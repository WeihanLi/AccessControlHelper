using EntityFramework.DbDescriptionHelper;
using System;

namespace PowerControlDemo.Models
{
    /// <summary>
    /// 用户角色表
    /// </summary>
    [TableDescription("ShopUserRole", "用户角色表")]
    public class ShopUserRoleModel : BaseModel
    {
        private string roleName;

        [ColumnDescription("角色名称")]
        public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }

        private string roleDesc;

        [ColumnDescription("角色描述")]
        public string RoleDesc
        {
            get { return roleDesc; }
            set { roleDesc = value; }
        }
    }
}