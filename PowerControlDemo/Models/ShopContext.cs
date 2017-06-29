using System;
using System.Data.Entity;
using System.Reflection;
using System.Text;
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
            modelBuilder.Entity<ShopRouteInfoModel>().ToTable("ShopRouteInfo");

            modelBuilder.Entity<ShopUserModel>().HasKey(s => s.PKID);
            modelBuilder.Entity<ShopUserAccessModel>().HasKey(s => s.PKID);
            modelBuilder.Entity<ShopUserRoleModel>().HasKey(s => s.PKID);
            modelBuilder.Entity<ShopUserRoleAccessModel>().HasKey(s => s.PKID);
            modelBuilder.Entity<ShopUserRoleMappingModel>().HasKey(s => s.PKID);
            modelBuilder.Entity<ShopAccessConfigModel>().HasKey(s => s.PKID);
            modelBuilder.Entity<ShopRouteInfoModel>().HasKey(m => m.PKID);

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<ShopUserModel> ShopUsers { get; set; }

        public virtual DbSet<ShopUserAccessModel> ShopUserAccesses { get; set; }

        public virtual DbSet<ShopUserRoleModel> ShopUserRoles { get; set; }

        public virtual DbSet<ShopUserRoleAccessModel> ShopUserRoleAccesses { get; set; }

        public virtual DbSet<ShopUserRoleMappingModel> ShopUserRoleMapping { get; set; }

        public virtual DbSet<ShopAccessConfigModel> ShopAccessConfigs { get; set; }

        public virtual  DbSet<ShopRouteInfoModel> ShopRouteInfo { get; set; }
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
            Guid uuid = Guid.NewGuid(), uuid1 = Guid.NewGuid();
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
            GenerateSqlDescriptionText(context);
        }

        public static void GenerateSqlDescriptionText(ShopContext context)
        {
            // generate description info
            StringBuilder sbSqlDescText = new StringBuilder();
            Type t = typeof(BaseModel);
            Assembly assembly = t.Assembly;
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (t.IsAssignableFrom(type) && (type.FullName!=t.FullName))
                {
                    //
                    var attribute = type.GetCustomAttribute(typeof(TableDescriptionAttribute)) as TableDescriptionAttribute;
                    string tableName = "", tableDesc = "";
                    if (attribute != null)
                    {
                        tableName = attribute.Name;
                        tableDesc = attribute.Description;
                    }
                    if (String.IsNullOrEmpty(tableName))
                    {
                        tableName = type.Name;
                    }
                    if (!String.IsNullOrEmpty(tableDesc))
                    {
                        //生成表描述sql
                        sbSqlDescText.AppendFormat(tableDescFormat, tableName, tableDesc);
                        sbSqlDescText.AppendLine();
                    }
                    var properties = type.GetProperties();
                    foreach (var property in properties)
                    {
                        var columnAttribute = property.GetCustomAttribute(typeof(ColumnDescriptionAttribute)) as ColumnDescriptionAttribute;
                        if (columnAttribute != null)
                        {
                            string columnName = columnAttribute.Name, columnDesc = columnAttribute.Description;
                            if (String.IsNullOrEmpty(columnDesc))
                            {
                                continue;
                            }
                            if (String.IsNullOrEmpty(columnName))
                            {
                                columnName = property.Name;
                            }
                            // 生成字段描述
                            sbSqlDescText.AppendFormat(columnDescFormat, tableName, columnName, columnDesc);
                            sbSqlDescText.AppendLine();
                        }
                    }
                }
            }
            if (sbSqlDescText.Length>0)
            {
                context.Database.ExecuteSqlCommand(sbSqlDescText.ToString());
            }
        }

        /// <summary>
        /// 表描述
        /// 0：表名称
        /// 1：表描述
        /// </summary>
        private static readonly string tableDescFormat = @"
BEGIN
IF EXISTS (
       SELECT 1
    FROM sys.extended_properties p,
         sys.tables t,
         sys.schemas s
    WHERE t.schema_id = s.schema_id
          AND p.major_id = t.object_id
          AND p.minor_id = 0
          AND p.name = N'MS_Description'
          AND s.name = N'dbo'
          AND t.name = N'{0}'
    )
        EXECUTE sp_updateextendedproperty N'MS_Description', N'{1}', N'SCHEMA', N'dbo',  N'TABLE', N'{0}';
ELSE
        EXECUTE sp_addextendedproperty N'MS_Description', N'{1}', N'SCHEMA', N'dbo',  N'TABLE', N'{0}'; 
END";

        /// <summary>
        /// 列描述信息
        /// 0：表名称，1：列名称，2：列描述信息
        /// </summary>
        private static readonly string columnDescFormat = @"
BEGIN
IF EXISTS (
        select 1
        from
            sys.extended_properties p, 
            sys.columns c, 
            sys.tables t, 
            sys.schemas s
        where
            t.schema_id = s.schema_id and
            c.object_id = t.object_id and
            p.major_id = t.object_id and
            p.minor_id = c.column_id and
            p.name = N'MS_Description' and 
            s.name = N'dbo' and
            t.name = N'{0}' and
            c.name = N'{1}'
    )
        EXECUTE sp_updateextendedproperty N'MS_Description', N'{2}', N'SCHEMA', N'dbo',  N'TABLE', N'{0}', N'COLUMN', N'{1}'; 
ELSE
        EXECUTE sp_addextendedproperty N'MS_Description', N'{2}', N'SCHEMA', N'dbo',  N'TABLE', N'{0}', N'COLUMN', N'{1}'; 
END";

    }
}