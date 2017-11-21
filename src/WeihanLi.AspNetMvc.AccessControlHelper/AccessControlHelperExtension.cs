using System;
using Autofac;
using WeihanLi.AspNetMvc.AccessControlHelper;
using WeihanLi.Common;

#if NET45

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    public static class AccessControlHelperExtensions
    {
        public static void AddAccessControlHelper<TActionStragety, TControlStragety>(this ContainerBuilder builder)
            where TActionStragety : class, IActionAccessStrategy
            where TControlStragety : class, IControlAccessStrategy
        {
            builder.RegisterType<TActionStragety>().As<IActionAccessStrategy>();
            builder.RegisterType<TControlStragety>().As<IControlAccessStrategy>();

            DependencyResolver.SetDependencyResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}
#else
using Microsoft.Extensions.DependencyInjection;

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
        public static AccessControlBuilder AddAccessControlHelper<TActionStragety, TControlStragety>(this IServiceCollection services)
            where TActionStragety : class, IActionAccessStrategy
            where TControlStragety : class, IControlAccessStrategy
        {
            services.AddSingleton<IActionAccessStrategy, TActionStragety>();
            services.AddSingleton<IControlAccessStrategy, TControlStragety>();
            // SetDependencyResolver
            DependencyResolver.SetDependencyResolver(new MicrosoftExtensionDependencyResolver(services.BuildServiceProvider()));
            return new AccessControlBuilder(services);
        }
    }
}
#endif