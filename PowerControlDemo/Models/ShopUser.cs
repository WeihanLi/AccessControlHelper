namespace PowerControlDemo.Models
{
    public class ShopUserModel : BaseModel
    {
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string passwordHash;

        public string PasswordHash
        {
            get { return passwordHash; }
            set { passwordHash = value; }
        }

        private string mobile;

        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
    }
}