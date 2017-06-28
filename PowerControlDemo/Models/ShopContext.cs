using System;
using System.Data.Entity;
using WeihanLi.Common.Helpers;

namespace PowerControlDemo.Models
{
    public class ShopContext : DbContext
    {
        public ShopContext() : base("name=ShopPowerContext")
        {
            Database.SetInitializer(new ShopPowerDbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShopUserModel>().ToTable("ShopUser");
            modelBuilder.Entity<ShopUserAccessModel>().ToTable("ShopUserAccess");
            modelBuilder.Entity<ShopUserRoleModel>().ToTable("ShopUserRole");
            modelBuilder.Entity<ShopUserRoleAccessModel>().ToTable("ShopUserRoleAccess");
            modelBuilder.Entity<ShopUserRoleMappingModel>().ToTable("ShopUserRoleMapping");
            modelBuilder.Entity<ShopAccessConfigModel>().ToTable("ShopAccessConfig");

            modelBuilder.Entity<ShopUserModel>().HasKey(s => s.PKID);
            modelBuilder.Entity<ShopUserAccessModel>().HasKey(s => s.PKID);
            modelBuilder.Entity<ShopUserRoleModel>().HasKey(s => s.PKID);
            modelBuilder.Entity<ShopUserRoleAccessModel>().HasKey(s => s.PKID);
            modelBuilder.Entity<ShopUserRoleMappingModel>().HasKey(s => s.PKID);
            modelBuilder.Entity<ShopAccessConfigModel>().HasKey(s => s.PKID); 

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<ShopUserModel> ShopUsers { get; set; }

        public virtual DbSet<ShopUserAccessModel> ShopUserAccesses { get; set; }

        public virtual DbSet<ShopUserRoleModel> ShopUserRoles { get; set; }

        public virtual DbSet<ShopUserRoleAccessModel> ShopUserRoleAccesses { get; set; }

        public virtual DbSet<ShopUserRoleMappingModel> ShopUserRoleMapping { get; set; }

        public virtual DbSet<ShopAccessConfigModel> ShopAccessConfigs { get; set; }
    }

    internal class ShopPowerDbInitializer : DropCreateDatabaseIfModelChanges<ShopContext>
    {
        public override void InitializeDatabase(ShopContext context)
        {
            //数据库初始化，不存在则创建
            if (!context.Database.Exists())
            {
                context.Database.Create();
                //初始化数据
                InitData(context);
                //update db
                context.SaveChanges();
            }
            else
            {
                base.InitializeDatabase(context);
            }
        }        

        protected override void Seed(ShopContext context)
        {
            InitData(context);
        }

        /// <summary>
        /// 初始化数据库中数据
        /// </summary>
        /// <param name="context">数据上下文</param>
        private static void InitData(ShopContext context)
        {
            // 数据初始化
            // user
            Guid uuid = Guid.NewGuid(),uuid1=Guid.NewGuid();
            var users = new ShopUserModel[]
            {
                new ShopUserModel{  UserName = "dm00038",UserGuid=Guid.NewGuid(),Email="dm00038@tuhu.cn", Mobile = "18300609893",PasswordHash = HashHelper.GetHashedString(HashType.SHA256,"12345678"), CreatedBy = "liweihan",CreatedTime = DateTime.Now, UpdatedBy="liweihan",UpdatedTime=DateTime.Now },
                new ShopUserModel{  UserName = "liweihan",UserGuid=uuid,Email="liweihan@tuhu.cn", Mobile = "13298393395",PasswordHash = HashHelper.GetHashedString(HashType.SHA256,"12345678"), CreatedBy = "liweihan",CreatedTime = DateTime.Now, UpdatedBy="liweihan",UpdatedTime=DateTime.Now },
                new ShopUserModel{  UserName = "liweihan1",Email="liweihan1@tuhu.cn",UserGuid=Guid.NewGuid(), Mobile = "13298393396",PasswordHash = HashHelper.GetHashedString(HashType.SHA256,"12345678"), CreatedBy = "liweihan",CreatedTime = DateTime.Now, UpdatedBy="liweihan",UpdatedTime=DateTime.Now },
                new ShopUserModel{  UserName = "gs00015",Email="gs00015@tuhu.cn",UserGuid=Guid.NewGuid(),  Mobile = "12345678901",PasswordHash = HashHelper.GetHashedString(HashType.SHA256,"12345678"), CreatedBy = "liweihan",CreatedTime = DateTime.Now, UpdatedBy="liweihan",UpdatedTime=DateTime.Now },
                new ShopUserModel{  UserName = "root",Email="root@tuhu.cn",UserGuid=uuid1, Mobile = "13000000000",PasswordHash = HashHelper.GetHashedString(HashType.SHA256,"12345678"), CreatedBy = "liweihan",CreatedTime = DateTime.Now, UpdatedBy="liweihan",UpdatedTime=DateTime.Now },
            };
            context.ShopUsers.AddRange(users);
            // user role
            var roles = new ShopUserRoleModel[]
            {
                new ShopUserRoleModel{ RoleName = "门店超级管理员", RoleDesc = "管理员 ###@tuhu.cn", CreatedBy = "liweihan",CreatedTime = DateTime.Now, UpdatedBy="liweihan",UpdatedTime=DateTime.Now },
                new ShopUserRoleModel{ RoleName = "门店店主", RoleDesc = "门店店主账号 dm###@tuhu.cn", CreatedBy = "liweihan",CreatedTime = DateTime.Now, UpdatedBy="liweihan",UpdatedTime=DateTime.Now },
                new ShopUserRoleModel{ RoleName = "公司", RoleDesc = "公司账号 gs###@tuhu.cn", CreatedBy = "liweihan",CreatedTime = DateTime.Now, UpdatedBy="liweihan",UpdatedTime=DateTime.Now },
                new ShopUserRoleModel{ RoleName = "研发管理员", RoleDesc = "研发 ###@tuhu.cn", CreatedBy = "liweihan",CreatedTime = DateTime.Now, UpdatedBy="liweihan",UpdatedTime=DateTime.Now },               
            };
            context.ShopUserRoles.AddRange(roles);
            // user role mapping
            var mappings = new ShopUserRoleMappingModel[]
            {
                new ShopUserRoleMappingModel{ UserName = "dm",MappingRule =1,RoleId = "2" , CreatedBy = "liweihan",CreatedTime = DateTime.Now, UpdatedBy="liweihan",UpdatedTime=DateTime.Now},
                new ShopUserRoleMappingModel{ UserName = "gs",MappingRule =1,RoleId = "3" , CreatedBy = "liweihan",CreatedTime = DateTime.Now, UpdatedBy="liweihan",UpdatedTime=DateTime.Now},
                new ShopUserRoleMappingModel{  UserId = uuid,UserName = "liweihan",MappingRule =0,RoleId = "4" , CreatedBy = "liweihan",CreatedTime = DateTime.Now, UpdatedBy="liweihan",UpdatedTime=DateTime.Now},
                new ShopUserRoleMappingModel
                {
                    UserId = uuid1,UserName = "root",MappingRule =0,RoleId = "1" , CreatedBy = "liweihan",CreatedTime = DateTime.Now, UpdatedBy="liweihan",UpdatedTime=DateTime.Now
                }
            };
            context.ShopUserRoleMapping.AddRange(mappings);
            // accessConfig
            var accessConfigs = new ShopAccessConfigModel[]
            {
                new ShopAccessConfigModel{ ParentId = 0, ActionName = "Contact",ControllerName = "Home", AreaName = "", AccessKey = Guid.NewGuid(), DisplayType = 1,ControlType=0, MenuName="Contact", CreatedBy = "liweihan",CreatedTime = DateTime.Now, UpdatedBy="liweihan",UpdatedTime=DateTime.Now }
            };
            context.ShopAccessConfigs.AddRange(accessConfigs);
            // role config
            var roleConfigs = new ShopUserRoleAccessModel[]
            {
                new ShopUserRoleAccessModel{ AccessId = 1, RoleId = 2, CreatedBy = "liweihan",CreatedTime = DateTime.Now, UpdatedBy="liweihan",UpdatedTime=DateTime.Now}
            };
            context.ShopUserRoleAccesses.AddRange(roleConfigs);
            // user config
            var userConfigs = new ShopUserAccessModel[]
            {
                new ShopUserAccessModel{ AccessId = 1, UserId = uuid, CreatedBy = "liweihan",CreatedTime = DateTime.Now, UpdatedBy="liweihan",UpdatedTime=DateTime.Now}
            };
            context.ShopUserAccesses.AddRange(userConfigs);            
        }
    }
}