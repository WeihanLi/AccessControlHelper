using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    internal class ServiceResolver
    {
        private static IServiceProvider _serviceProvider;

        static ServiceResolver()
        {
            _serviceProvider = new DefaultServiceProvider();
        }

        public static IServiceProvider Current => _serviceProvider;

        public static void SetReslover(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static void SetReslover(Func<Type, object> getService)
        {
            _serviceProvider = new DelegateServiceProvider(getService);
        }

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

            public DelegateServiceProvider(Func<Type, object> func)
            => _func = func ?? throw new ArgumentNullException(nameof(func));

            public object GetService(Type serviceType)
                => _func(serviceType);
        }
    }

    internal static class ServiceResolverExtensions
    {
        public static TService GetService<TService>(this IServiceProvider serviceProvider)
            => (TService)serviceProvider.GetService(typeof(TService));
    }
}