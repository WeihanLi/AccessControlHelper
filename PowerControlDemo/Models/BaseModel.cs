using System;

namespace PowerControlDemo.Models
{
    public class BaseModel
    {
        private int pkid;

        /// <summary>
        /// 主键
        /// </summary>
        [ColumnDescription("主键")]
        public int PKID
        {
            get { return pkid; }
            set { pkid = value; }
        }

        private bool isDeleted;

        /// <summary>
        /// 是否删除
        /// </summary>
        [ColumnDescription("是否删除")]
        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

        private string createdBy;

        /// <summary>
        /// 创建人
        /// </summary>
        [ColumnDescription("创建人")]
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        private DateTime createdTime;

        /// <summary>
        /// 创建时间
        /// </summary>
        [ColumnDescription("创建时间")]
        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        private string updatedBy;

        /// <summary>
        /// 更新人
        /// </summary>
        [ColumnDescription("更新人")]
        public string UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }

        private DateTime updatedTime;

        /// <summary>
        /// 更新时间
        /// </summary>
        [ColumnDescription("更新时间")]
        public DateTime UpdatedTime
        {
            get { return updatedTime; }
            set { updatedTime = value; }
        }
    }
}