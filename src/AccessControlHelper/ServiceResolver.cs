using System;

#if !NET45

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

#endif

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    internal sealed class ServiceResolver
    {
        private static readonly object _locker = new object();

        static ServiceResolver()
        {
            Current = new DefaultServiceProvider();
        }

        public static IServiceProvider Current { get; private set; }

        public static void SetResolver(IServiceProvider serviceProvider)
        {
            lock (_locker)
            {
                Current = serviceProvider;
            }
        }

        public static void SetResolver(Func<Type, object> getService) => SetResolver(new DelegateServiceProvider(getService));

        private class DefaultServiceProvider : IServiceProvider
        {
            public object GetService(Type serviceType)
            {
                if (serviceType.IsInterface || serviceType.IsAbstract)
                {
                    return null;
                }
                try
                {
                    return Activator.CreateInstance(serviceType);
                }
                catch
                {
                    return null;
                }
            }
        }

        private class DelegateServiceProvider : IServiceProvider
        {
            private readonly Func<Type, object> _func;

            public DelegateServiceProvider(Func<Type, object> func) => _func = func ?? throw new ArgumentNullException(nameof(func));

            public object GetService(Type serviceType) => _func(serviceType);
        }
    }

    internal static class ServiceResolverExtensions
    {
        public static TService ResolveService<TService>(this IServiceProvider serviceProvider)
        {
            if (null == serviceProvider)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

#if NET45
            return (TService) serviceProvider.GetService(typeof(TService));
#else
            var accessor = serviceProvider.GetService<IHttpContextAccessor>();
            if (accessor != null)
            {
                return accessor.HttpContext.RequestServices.GetService<TService>();
            }
            return serviceProvider.GetService<TService>();
#endif
        }
    }
}
