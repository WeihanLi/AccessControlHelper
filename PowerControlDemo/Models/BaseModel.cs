using System;

namespace PowerControlDemo.Models
{
    public class BaseModel
    {
        private int pkid;

        public int PKID
        {
            get { return pkid; }
            set { pkid = value; }
        }

        private bool isDeleted;

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

        private string createdBy;

        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        private DateTime createdTime;

        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        private string updatedBy;

        public string UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }

        private DateTime updatedTime;

        public DateTime UpdatedTime
        {
            get { return updatedTime; }
            set { updatedTime = value; }
        }
    }
}