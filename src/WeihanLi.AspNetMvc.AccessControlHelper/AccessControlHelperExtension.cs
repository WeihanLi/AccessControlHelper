using System;
using WeihanLi.Common;

#if NET45
using Autofac;
namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    public static class AccessControlHelperExtensions
    {
        public static void RegisterAccessControlHelper<TActionStragety, TControlStragety>(this ContainerBuilder builder)
            where TActionStragety : class, IActionAccessStrategy
            where TControlStragety : class, IControlAccessStrategy
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.RegisterType<TActionStragety>().As<IActionAccessStrategy>();
            builder.RegisterType<TControlStragety>().As<IControlAccessStrategy>();

            DependencyResolver.SetDependencyResolver(new AutofacDependencyResolver(builder.Build()));
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
            // SetDependencyResolver
            DependencyResolver.SetDependencyResolver(new MicrosoftExtensionDependencyResolver(services.BuildServiceProvider()));
            return new AccessControlHelperBuilder(services);
        }
    }
}

#endif