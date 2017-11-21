using Microsoft.Extensions.DependencyInjection;

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    public interface IAccessControlBuilder
    {
        IServiceCollection Services { get; }
    }

    public class AccessControlBuilder : IAccessControlBuilder
    {
        public IServiceCollection Services { get; }

        public AccessControlBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}