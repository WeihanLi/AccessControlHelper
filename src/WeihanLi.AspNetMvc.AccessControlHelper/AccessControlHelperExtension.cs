using System;

#if NET45
namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    public static class AccessControlHelper
    {
        public static void RegisterAccessControlHelper<TActionStragety, TControlStragety>()
            where TActionStragety : class, IActionAccessStrategy
            where TControlStragety : class, IControlAccessStrategy
        {
            IocContainer.DefaultContainer.Register<IActionAccessStrategy, TActionStragety>();
            IocContainer.DefaultContainer.Register<IControlAccessStrategy, TControlStragety>();

        }
    }
}
#else

using WeihanLi.AspNetMvc.AccessControlHelper;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAccessControlHelper(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            return app.UseMiddleware<AccessControlHelperMiddleware>();
        }
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static AccessControlHelperBuilder AddAccessControlHelper<TActionStragety, TControlStragety>(this IServiceCollection services)
            where TActionStragety : class, IActionAccessStrategy
            where TControlStragety : class, IControlAccessStrategy
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddSingleton<IActionAccessStrategy, TActionStragety>();
            services.AddSingleton<IControlAccessStrategy, TControlStragety>();
            //
            IocContainer.DefaultContainer.Register<IActionAccessStrategy, TActionStragety>();
            IocContainer.DefaultContainer.Register<IControlAccessStrategy, TControlStragety>();

            return new AccessControlHelperBuilder(services);
        }
    }
}

#endif