using System;

namespace PowerControlDemo.Models
{
    [TableDescription("ShopUserAccess","用户访问权限")]
    public class ShopUserAccessModel : BaseModel
    {
        private Guid userId;

        /// <summary>
        /// 用户Guid
        /// </summary>
        [ColumnDescription("用户Guid")]
        public Guid UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private long accessId;

        /// <summary>
        /// 权限控制id
        /// </summary>
        [ColumnDescription("权限控制id")]
        public long AccessId
        {
            get { return accessId; }
            set { accessId = value; }
        }
    }
}