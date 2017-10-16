using System.Data.Entity;

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

        public virtual DbSet<ShopRouteInfoModel> ShopRouteInfo { get; set; }
    }
}