using System;

#if NET45
namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    public static class AccessControlHelper
    {
        public static void RegisterAccessControlHelper<TActionStragety, TControlStragety>(Func<IServiceProvider> registerFunc)
            where TActionStragety : class, IResourceAccessStrategy
            where TControlStragety : class, IControlAccessStrategy
        {
            ServiceResolver.SetResolver(registerFunc());
        }

        public static void RegisterAccessControlHelper<TActionStragety, TControlStragety>(Func<Type, object> getServiceFunc)
            where TActionStragety : class, IResourceAccessStrategy
            where TControlStragety : class, IControlAccessStrategy
        {
            ServiceResolver.SetResolver(getServiceFunc);
        }

        public static void RegisterAccessControlHelper<TActionStragety, TControlStragety>(Action<Type, Type> registerTypeAsAction, Func<Type, object> getServiceFunc)
            where TActionStragety : class, IResourceAccessStrategy
            where TControlStragety : class, IControlAccessStrategy
        {
            registerTypeAsAction(typeof(TActionStragety), typeof(IResourceAccessStrategy));
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
        public static AccessControlHelperBuilder AddAccessControlHelper<TResourceStrategy, TControlStrategy>(this IServiceCollection services)
            where TResourceStrategy : class, IResourceAccessStrategy
            where TControlStrategy : class, IControlAccessStrategy
        {
            return services.AddAccessControlHelper<TResourceStrategy, TControlStrategy>(options => { });
        }

        public static AccessControlHelperBuilder AddAccessControlHelper<TResourceStrategy, TControlStrategy>(this IServiceCollection services, Action<AccessControlOption> configAction)
            where TResourceStrategy : class, IResourceAccessStrategy
            where TControlStrategy : class, IControlAccessStrategy
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configAction == null)
            {
                configAction = options => { };
            }

            services.Configure(configAction); // configAction

            services.AddAuthorization(options => options.AddPolicy("AccessControl", new AuthorizationPolicyBuilder().AddRequirements(new AccessControlRequirement()).Build()));
            services.AddSingleton<IAuthorizationHandler, AccessControlAuthorizationHandler>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IResourceAccessStrategy, TResourceStrategy>();
            services.TryAddSingleton<IControlAccessStrategy, TControlStrategy>();

            ServiceResolver.SetResolver(services.BuildServiceProvider());
            return new AccessControlHelperBuilder(services);
        }
    }
}

#endif
