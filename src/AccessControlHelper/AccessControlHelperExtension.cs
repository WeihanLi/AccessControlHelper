using System;

#if NET45
using WeihanLi.Common;

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    public static class AccessControlHelper
    {
        public static void RegisterAccessControlHelper<TResourceStrategy, TControlStrategy>(Func<IServiceProvider> registerFunc)
            where TResourceStrategy : class, IResourceAccessStrategy
            where TControlStrategy : class, IControlAccessStrategy
        {
            DependencyResolver.SetDependencyResolver(registerFunc());
        }

        public static void RegisterAccessControlHelper<TResourceStrategy, TControlStrategy>(Func<Type, object> getServiceFunc)
            where TResourceStrategy : class, IResourceAccessStrategy
            where TControlStrategy : class, IControlAccessStrategy
        {
            DependencyResolver.SetDependencyResolver(getServiceFunc);
        }

        public static void RegisterAccessControlHelper<TResourceStrategy, TControlStrategy>(Action<Type, Type> registerTypeAsAction, Func<Type, object> getServiceFunc)
            where TResourceStrategy : class, IResourceAccessStrategy
            where TControlStrategy : class, IControlAccessStrategy
        {
            registerTypeAsAction(typeof(TResourceStrategy), typeof(IResourceAccessStrategy));
            registerTypeAsAction(typeof(TControlStrategy), typeof(IControlAccessStrategy));

            DependencyResolver.SetDependencyResolver(getServiceFunc);
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

            services.TryAddSingleton<IResourceAccessStrategy, TResourceAccessStrategy>();
            services.TryAddSingleton<IControlAccessStrategy, TControlStrategy>();

            return services.AddAccessControlHelper();
        }

        public static IAccessControlHelperBuilder AddAccessControlHelper<TResourceAccessStrategy, TControlStrategy>(this IServiceCollection services, ServiceLifetime resourceAccessStrategyLifetime, ServiceLifetime controlAccessStrategyLifetime)
            where TResourceAccessStrategy : class, IResourceAccessStrategy
            where TControlStrategy : class, IControlAccessStrategy
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(new ServiceDescriptor(typeof(IResourceAccessStrategy), typeof(TResourceAccessStrategy), resourceAccessStrategyLifetime));
            services.TryAdd(new ServiceDescriptor(typeof(IControlAccessStrategy), typeof(TControlStrategy), controlAccessStrategyLifetime));

            return services.AddAccessControlHelper();
        }

        public static IAccessControlHelperBuilder AddAccessControlHelper<TResourceAccessStrategy, TControlStrategy>(this IServiceCollection services, Action<AccessControlOptions> configAction)
            where TResourceAccessStrategy : class, IResourceAccessStrategy
            where TControlStrategy : class, IControlAccessStrategy
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configAction != null)
            {
                services.Configure(configAction);
            }
            return services.AddAccessControlHelper<TResourceAccessStrategy, TControlStrategy>();
        }

        public static IAccessControlHelperBuilder AddAccessControlHelper<TResourceAccessStrategy, TControlStrategy>(this IServiceCollection services, Action<AccessControlOptions> configAction, ServiceLifetime resourceAccessStrategyLifetime, ServiceLifetime controlAccessStrategyLifetime)
            where TResourceAccessStrategy : class, IResourceAccessStrategy
            where TControlStrategy : class, IControlAccessStrategy
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configAction != null)
            {
                services.Configure(configAction);
            }
            return services.AddAccessControlHelper<TResourceAccessStrategy, TControlStrategy>(resourceAccessStrategyLifetime, controlAccessStrategyLifetime);
        }

        public static IAccessControlHelperBuilder AddAccessControlHelper(this IServiceCollection services, bool useAsDefaultPolicy)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (useAsDefaultPolicy)
            {
                services.AddAuthorization(options =>
                {
                    var accessControlPolicy = new AuthorizationPolicyBuilder()
                        .AddRequirements(new AccessControlRequirement())
                        .Build();
                    options.AddPolicy(AccessControlHelperConstants.PolicyName, accessControlPolicy);
                    options.DefaultPolicy = accessControlPolicy;
                });
            }
            else
            {
                services.AddAuthorization(options =>
                {
                    var accessControlPolicy = new AuthorizationPolicyBuilder()
                        .AddRequirements(new AccessControlRequirement())
                        .Build();
                    options.AddPolicy(AccessControlHelperConstants.PolicyName, accessControlPolicy);
                });
            }

            services.AddSingleton<IAuthorizationHandler, AccessControlAuthorizationHandler>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return new AccessControlHelperBuilder(services);
        }

        public static IAccessControlHelperBuilder AddAccessControlHelper(this IServiceCollection services)
        {
            return AddAccessControlHelper(services, false);
        }

        public static IAccessControlHelperBuilder AddAccessControlHelper(this IServiceCollection services, Action<AccessControlOptions> configAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var useAsDefaultPolicy = false;
            if (configAction != null)
            {
                var option = new AccessControlOptions();
                configAction.Invoke(option);
                useAsDefaultPolicy = option.UseAsDefaultPolicy;

                services.Configure(configAction);
            }
            return services.AddAccessControlHelper(useAsDefaultPolicy);
        }

        public static IAccessControlHelperBuilder AddResourceAccessStrategy<TResourceAccessStrategy>(this IAccessControlHelperBuilder builder) where TResourceAccessStrategy : IResourceAccessStrategy
        {
            return AddResourceAccessStrategy<TResourceAccessStrategy>(builder, ServiceLifetime.Singleton);
        }

        public static IAccessControlHelperBuilder AddResourceAccessStrategy<TResourceAccessStrategy>(this IAccessControlHelperBuilder builder, ServiceLifetime serviceLifetime) where TResourceAccessStrategy : IResourceAccessStrategy
        {
            if (null == builder)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Add(
                new ServiceDescriptor(typeof(IResourceAccessStrategy), typeof(TResourceAccessStrategy), serviceLifetime));
            return builder;
        }

        public static IAccessControlHelperBuilder AddControlAccessStrategy<TControlAccessStrategy>(this IAccessControlHelperBuilder builder) where TControlAccessStrategy : IControlAccessStrategy
        {
            return AddControlAccessStrategy<TControlAccessStrategy>(builder, ServiceLifetime.Singleton);
        }

        public static IAccessControlHelperBuilder AddControlAccessStrategy<TControlAccessStrategy>(this IAccessControlHelperBuilder builder, ServiceLifetime serviceLifetime) where TControlAccessStrategy : IControlAccessStrategy
        {
            if (null == builder)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Add(new ServiceDescriptor(typeof(IControlAccessStrategy), typeof(TControlAccessStrategy), serviceLifetime));
            return builder;
        }
    }
}

#endif
