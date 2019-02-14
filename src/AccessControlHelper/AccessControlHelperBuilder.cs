using Microsoft.Extensions.DependencyInjection;

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    public interface IAccessControlHelperBuilder
    {
        IServiceCollection Services { get; }
    }

    internal class AccessControlHelperBuilder : IAccessControlHelperBuilder
    {
        public IServiceCollection Services { get; }

        public AccessControlHelperBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}