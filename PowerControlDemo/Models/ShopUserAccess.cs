namespace PowerControlDemo.Models
{
    public class ShopUserAccessModel : BaseModel
    {
        private int userId;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private long accessId;

        public long AccessId
        {
            get { return accessId; }
            set { accessId = value; }
        }
    }
}