using PowerControlDemo.DataAccess;
namespace PowerControlDemo.Business
{
    public interface IBusinessHelper
    {
        DALShopAccessConfig ShopAccessConfigHelper { get; }
        DALShopUser ShopUserHelper { get; }
        DALShopUserAccess ShopUserAccessHelper { get; }
        DALShopUserRole ShopUserRoleHelper { get; }
        DALShopUserRoleAccess ShopUserRoleAccessHelper { get; }
        DALShopUserRoleMapping ShopUserRoleMappingHelper { get; }
        
    }

    public class BusinessHelper : IBusinessHelper
    {
        private DALShopAccessConfig _ShopAccessConfigHelper;        
        /// <summary>
        /// ShopAccessConfigHelper
        /// </summary>
	    public DALShopAccessConfig ShopAccessConfigHelper
        {
            get
            {
                if(_ShopAccessConfigHelper == null)
                {
                    _ShopAccessConfigHelper = new DALShopAccessConfig();
                }
                return _ShopAccessConfigHelper;
            }
        }

        private DALShopUser _ShopUserHelper;        
        /// <summary>
        /// ShopUserHelper
        /// </summary>
	    public DALShopUser ShopUserHelper
        {
            get
            {
                if(_ShopUserHelper == null)
                {
                    _ShopUserHelper = new DALShopUser();
                }
                return _ShopUserHelper;
            }
        }

        private DALShopUserAccess _ShopUserAccessHelper;        
        /// <summary>
        /// ShopUserAccessHelper
        /// </summary>
	    public DALShopUserAccess ShopUserAccessHelper
        {
            get
            {
                if(_ShopUserAccessHelper == null)
                {
                    _ShopUserAccessHelper = new DALShopUserAccess();
                }
                return _ShopUserAccessHelper;
            }
        }

        private DALShopUserRole _ShopUserRoleHelper;        
        /// <summary>
        /// ShopUserRoleHelper
        /// </summary>
	    public DALShopUserRole ShopUserRoleHelper
        {
            get
            {
                if(_ShopUserRoleHelper == null)
                {
                    _ShopUserRoleHelper = new DALShopUserRole();
                }
                return _ShopUserRoleHelper;
            }
        }

        private DALShopUserRoleAccess _ShopUserRoleAccessHelper;        
        /// <summary>
        /// ShopUserRoleAccessHelper
        /// </summary>
	    public DALShopUserRoleAccess ShopUserRoleAccessHelper
        {
            get
            {
                if(_ShopUserRoleAccessHelper == null)
                {
                    _ShopUserRoleAccessHelper = new DALShopUserRoleAccess();
                }
                return _ShopUserRoleAccessHelper;
            }
        }

        private DALShopUserRoleMapping _ShopUserRoleMappingHelper;        
        /// <summary>
        /// ShopUserRoleMappingHelper
        /// </summary>
	    public DALShopUserRoleMapping ShopUserRoleMappingHelper
        {
            get
            {
                if(_ShopUserRoleMappingHelper == null)
                {
                    _ShopUserRoleMappingHelper = new DALShopUserRoleMapping();
                }
                return _ShopUserRoleMappingHelper;
            }
        }

    }
}