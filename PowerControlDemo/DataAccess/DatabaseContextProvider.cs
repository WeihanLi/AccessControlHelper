using System.Data.Entity;

namespace PowerControlDemo.DataAccess
{
    public class DatabaseContextProvider
    {
        private static readonly object _contextLock = new object();
        private static DbContext _dbContext;

        public static DbContext GetDbContext()
        {
            if (_dbContext == null)
            {
                lock (_contextLock)
                {
                    if (_dbContext == null)
                    {
                        _dbContext = new Models.ShopContext();                       
                    }
                    return _dbContext;
                }
            }
            return _dbContext;
        }

        public static string[] GetTables()
        {
            return new string[]
            {
                "ShopUserModel",
                "ShopAccessConfigModel",
                "ShopUserAccessModel",
                "ShopUserRoleModel",
                "ShopUserRoleAccess",
                "ShopUserRoleMappingModel"
            };
        }
    }
}