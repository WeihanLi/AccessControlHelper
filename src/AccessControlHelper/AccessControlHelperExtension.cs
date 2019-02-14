using System;

#if NET45
namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    public static class AccessControlHelper
    {
        public static void RegisterAccessControlHelper<TResourceStragety, TControlStragety>(Func<IServiceProvider> registerFunc)
            where TResourceStragety : class, IResourceAccessStrategy
            where TControlStragety : class, IControlAccessStrategy
        {
            ServiceResolver.SetResolver(registerFunc());
        }

        public static void RegisterAccessControlHelper<TResourceStragety, TControlStragety>(Func<Type, object> getServiceFunc)
            where TResourceStragety : class, IResourceAccessStrategy
            where TControlStragety : class, IControlAccessStrategy
        {
            ServiceResolver.SetResolver(getServiceFunc);
        }

        public static void RegisterAccessControlHelper<TResourceStragety, TControlStragety>(Action<Type, Type> registerTypeAsAction, Func<Type, object> getServiceFunc)
            where TResourceStragety : class, IResourceAccessStrategy
            where TControlStragety : class, IControlAccessStrategy
        {
            registerTypeAsAction(typeof(TResourceStragety), typeof(IResourceAccessStrategy));
            registerTypeAsAction(typeof(TControlStragety), typeof(IControlAccessStrategy));

            ServiceResolver.SetResolver(getServiceFunc);
        }
    }
}
#else

using WeihanLi.AspNetMvc.AccessControlHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
        public static IAccessControlHelperBuilder AddAccessControlHelper<TResourceAccessStrategy, TControlStrategy>(this IServiceCollection services)
            where TResourceAccessStrategy : class, IResourceAccessStrategy
            where TControlStrategy : class, IControlAccessStrategy
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddAuthorization(options => options.AddPolicy("AccessControl", new AuthorizationPolicyBuilder().AddRequirements(new AccessControlRequirement()).Build()));
            services.AddSingleton<IAuthorizationHandler, AccessControlAuthorizationHandler>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IResourceAccessStrategy, TResourceAccessStrategy>();
            services.TryAddSingleton<IControlAccessStrategy, TControlStrategy>();
            ServiceResolver.SetResolver(services.BuildServiceProvider());

            return new AccessControlHelperBuilder(services);
        }

        public static IAccessControlHelperBuilder AddAccessControlHelper<TResourceAccessStrategy, TControlStrategy>(this IServiceCollection services, Action<AccessControlOption> configAction)
            where TResourceAccessStrategy : class, IResourceAccessStrategy
            where TControlStrategy : class, IControlAccessStrategy
        {
            if (configAction != null)
            {
                services.Configure(configAction); // configAction
            }
            return services.AddAccessControlHelper<TResourceAccessStrategy, TControlStrategy>();
        }
    }
}

#endif
