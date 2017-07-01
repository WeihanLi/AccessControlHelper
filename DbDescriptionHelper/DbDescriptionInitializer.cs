using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EntityFramework.DbDescriptionHelper
{
    /// <summary>
    /// 数据库描述信息Initializer
    /// </summary>
    public class DbDescriptionInitializer
    {
        /// <summary>
        /// 表描述
        /// 0：表名称
        /// 1：表描述
        /// </summary>
        private readonly string tableDescFormat = @"
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
        private readonly string columnDescFormat = @"
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

        /// <summary>
        /// 生成数据库
        /// </summary>
        /// <param name="dbContextTypeName"></param>
        /// <returns></returns>
        public virtual string GenerateDbDescriptionSqlText(Type contextType)
        {
            //// get the assembly
            //Assembly assembly = Assembly.GetCallingAssembly();
            //Type contextType;
            //if (String.IsNullOrEmpty(dbContextTypeName))
            //{
            //    contextType = assembly.GetType(dbContextTypeName);
            //}
            //else
            //{
            //    contextType = assembly.GetExportedTypes().Where(t => (typeof(DbContext)).IsAssignableFrom(t)).FirstOrDefault();
            //}
            if (contextType == null)
            {
                throw new ArgumentNullException(nameof(contextType), "contextType can not be null.");
            }
            if (!(typeof(DbContext)).IsAssignableFrom(contextType))
            {
                throw new ArgumentException("contextType should extends from DbContext.", nameof(contextType));
            }
            var types = contextType.GetProperties().Where(p =>
           p.PropertyType.IsGenericType &&
           p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>)).Select(p => p.PropertyType.GetGenericArguments().FirstOrDefault());
            if (types != null && types.Any())
            {
                StringBuilder sbSqlDescText = new StringBuilder();
                foreach (var type in types)
                {
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
                return sbSqlDescText.ToString();
            }
            return "";
        }

        /// <summary>
        /// 生成数据库描述信息
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文类型</typeparam>
        /// <param name="context">数据库上下文</param>
        public virtual void GenerateDbDescription<TDbContext>(TDbContext context) where TDbContext : DbContext
        {
            //get dbContext type
            var contextType = typeof(TDbContext);
            var types = contextType.GetProperties().Where(p =>
           p.PropertyType.IsGenericType &&
           p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>)).Select(p => p.PropertyType.GetGenericArguments().FirstOrDefault());
            if (types != null && types.Any())
            {
                StringBuilder sbSqlDescText = new StringBuilder();
                foreach (var type in types)
                {
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
                if (sbSqlDescText.Length > 0)
                {
                    context.Database.ExecuteSqlCommand(sbSqlDescText.ToString());
                }
            }
        }
    }
}