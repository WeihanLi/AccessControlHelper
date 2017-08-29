using System;

namespace AccessControlDemo.Services
{
    public class ControlAccessStrategy : AccessControlHelper.IControlAccessStrategy
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