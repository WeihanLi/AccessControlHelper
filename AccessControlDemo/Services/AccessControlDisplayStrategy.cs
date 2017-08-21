using System;

namespace AccessControlDemo.Services
{
    public class AccessControlDisplayStrategy : AccessControlHelper.IControlDisplayStrategy
    {
        public bool IsControlCanAccess(string accessKey)
        {
            if (!String.IsNullOrWhiteSpace(accessKey))
            {
                return true;
            }
            return false;
        }
    }
}