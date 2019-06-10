using System;
using WeihanLi.Common;

#if !NET45

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

#endif

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    internal static class ServiceResolverExtensions
    {
        public static TService ResolveCustomService<TService>(this IDependencyResolver serviceProvider)
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
