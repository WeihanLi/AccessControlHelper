namespace System
{
    /// <summary>
    /// 列描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnDescriptionAttribute : Attribute
    {
        public ColumnDescriptionAttribute(string desc)
        {
            Description = desc;
        }

        public string Description { get; private set; }
    }

    /// <summary>
    /// 表描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TableDescriptionAttribute : Attribute
    {
        public TableDescriptionAttribute(string desc)
        {
            Description = desc;
        }

        public string Description { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DatabaseDescriptionAttribute : Attribute
    {
        public DatabaseDescriptionAttribute(string desc)
        {
            Description = desc;
        }

        public string Description { get; private set; }
    }
}