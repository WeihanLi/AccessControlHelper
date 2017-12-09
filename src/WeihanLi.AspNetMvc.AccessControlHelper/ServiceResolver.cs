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
    }

    internal static class ServiceResolverExtensions
    {
        public static TService GetService<TService>(this IServiceProvider serviceProvider)
            => (TService)serviceProvider.GetService(typeof(TService));
    }
}