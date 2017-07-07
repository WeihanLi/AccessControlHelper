using EntityFramework.DbDescriptionHelper;
using System;

namespace PowerControlDemo.Models
{
    [TableDescription("ShopUser","用户表")]
    public class ShopUserModel : BaseModel
    {
        private Guid userGuid;

        /// <summary>
        /// 用户Guid
        /// </summary>
        [ColumnDescription("用户Guid")]
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        private string userName;

        /// <summary>
        /// 用户名
        /// </summary>
        [ColumnDescription("用户名")]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string passwordHash;

        /// <summary>
        /// 密码哈希值
        /// </summary>
        [ColumnDescription("密码")]
        public string PasswordHash
        {
            get { return passwordHash; }
            set { passwordHash = value; }
        }

        private string mobile;

        /// <summary>
        /// 手机号
        /// </summary>
        [ColumnDescription("手机号")]
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }

        private string email;

        /// <summary>
        /// 邮箱
        /// </summary>
        [ColumnDescription("邮箱")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
    }
}