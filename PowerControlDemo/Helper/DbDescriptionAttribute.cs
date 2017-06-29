using System.Data.Entity;
using System.Reflection;
using System.Text;
using PowerControlDemo.Models;

namespace System
{
    /// <summary>
    /// 列描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnDescriptionAttribute : Attribute
    {
        public ColumnDescriptionAttribute(string desc)
        {
            Description = desc;
        }
        public ColumnDescriptionAttribute(string name, string desc)
        {
            Name = name;
            Description = desc;
        }

        public string Name { get; }
        public string Description { get; private set; }
    }

    /// <summary>
    /// 表描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableDescriptionAttribute : Attribute
    {
        public TableDescriptionAttribute(string desc)
        {
            Description = desc;
        }

        public TableDescriptionAttribute(string name,string desc)
        {
            Name = name;
            Description = desc;
        }

        public string Name { get; }

        public string Description { get; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class DatabaseDescriptionAttribute : Attribute
    {
        public DatabaseDescriptionAttribute(string desc)
        {
            Description = desc;
        }
        public DatabaseDescriptionAttribute(string name, string desc)
        {
            Name = name;
            Description = desc;
        }

        public string Name { get; }
        public string Description { get; private set; }
    }

    /// <summary>
    /// 数据库描述接口
    /// </summary>
    /// <typeparam name="TDbContext">数据库上下文</typeparam>
    public interface IDbDescriptionInitializer<TDbContext> where TDbContext:DbContext
    {
        /// <summary>
        /// 生成SqlDescriptionText
        /// </summary>
        /// <param name="context">数据库上下文</param>
        /// <param name="baseModelType">typeof(BaseModel)</param>
        void GenerateSqlDescriptionText(TDbContext context, Type baseModelType);
    }

    /// <summary>
    /// 数据库描述信息生成
    /// </summary>
    /// <typeparam name="TDbContext">数据库上下文</typeparam>
    public class DbDescriptionInitializer<TDbContext> : IDbDescriptionInitializer<TDbContext> where TDbContext : DbContext
    {
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
        /// <summary>
        /// 生成数据库描述sql
        /// </summary>
        /// <param name="context">数据库上下文</param>
        public virtual void GenerateSqlDescriptionText(TDbContext context,Type baseModelType)
        {
            // generate description info
            StringBuilder sbSqlDescText = new StringBuilder();
            Type t = baseModelType;
            Assembly assembly = t.Assembly;
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (t.IsAssignableFrom(type) && (type.FullName != t.FullName))
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
            if (sbSqlDescText.Length > 0)
            {
                context.Database.ExecuteSqlCommand(sbSqlDescText.ToString());
            }
        }
    }
}