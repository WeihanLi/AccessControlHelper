using System;

namespace EntityFramework.DbDescriptionHelper
{
    /// <summary>
    /// 表描述信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class TableDescriptionAttribute : Attribute
    {
        public TableDescriptionAttribute(string desc)
        {
            Description = desc;
        }

        public TableDescriptionAttribute(string name, string desc)
        {
            Name = name;
            Description = desc;
        }

        /// <summary>
        /// 表名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 表描述
        /// </summary>
        public string Description { get; }
    }
}