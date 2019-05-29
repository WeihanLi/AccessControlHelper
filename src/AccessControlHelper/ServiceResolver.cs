using System;

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    internal sealed class ServiceResolver
    {
        private static IServiceProvider _serviceProvider;
        private static readonly object _locker = new object();

        static ServiceResolver()
        {
            _serviceProvider = new DefaultServiceProvider();
        }

        public static IServiceProvider Current => _serviceProvider;

        public static void SetResolver(IServiceProvider serviceProvider)
        {
            lock (_locker)
            {
                _serviceProvider = serviceProvider;
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

            public DelegateServiceProvider(Func<Type, object> func)
            => _func = func ?? throw new ArgumentNullException(nameof(func));

            public object GetService(Type serviceType)
                => _func(serviceType);
        }
    }

    internal static class ServiceResolverExtensions
    {
        public static TService ResolveService<TService>(this IServiceProvider serviceProvider) => (TService)serviceProvider.GetService(typeof(TService));
    }
}
