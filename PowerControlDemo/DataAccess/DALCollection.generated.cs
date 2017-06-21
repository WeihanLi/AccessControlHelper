 
using PowerControlDemo.Models;

namespace PowerControlDemo.DataAccess
{
	public partial class DALShopAccessConfig: BaseDaL<ShopAccessConfigModel> { }
	public partial class DALShopUser: BaseDaL<ShopUserModel> { }
	public partial class DALShopUserAccess: BaseDaL<ShopUserAccessModel> { }
	public partial class DALShopUserRole: BaseDaL<ShopUserRoleModel> { }
	public partial class DALShopUserRoleAccess: BaseDaL<ShopUserRoleAccessModel> { }
	public partial class DALShopUserRoleMapping: BaseDaL<ShopUserRoleMappingModel> { }
}