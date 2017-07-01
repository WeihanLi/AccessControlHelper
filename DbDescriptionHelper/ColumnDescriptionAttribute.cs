using System;

namespace EntityFramework.DbDescriptionHelper
{
    /// <summary>
    /// 列描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = false,Inherited =false)]
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

        /// <summary>
        /// 列名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 列描述信息
        /// </summary>
        public string Description { get; private set; }
    }
}