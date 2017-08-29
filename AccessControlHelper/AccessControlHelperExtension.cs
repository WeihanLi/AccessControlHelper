using System;

#if !NET45
using WeihanLi.AspNetMvc.AccessControlHelper;
using Microsoft.Extensions.Options;

#endif
#if NET45
namespace WeihanLi.AspNetMvc.AccessControlHelper
#else
namespace Microsoft.AspNetCore.Builder
#endif
{
    public static class AccessControlHelperExtensions
    {
#if NET45
        public static void RegisterAccessStragety(AccessControlHelperOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            AccessControlAttribute.RegisterAccessStrategy(options.ActionAccessStrategy);
            HtmlHelperExtension.RegisterAccessStrategy(options.ControlAccessStrategy);
        }

        public static void RegisterAccessStragety(Action<AccessControlHelperOptions> optionsAction)
        {
            if (optionsAction == null)
            {
                throw new ArgumentNullException(nameof(optionsAction));
            }
            AccessControlHelperOptions options = new AccessControlHelperOptions();
            optionsAction(options);
            AccessControlAttribute.RegisterAccessStrategy(options.ActionAccessStrategy);
            HtmlHelperExtension.RegisterAccessStrategy(options.ControlAccessStrategy);
        }
#else
        public static IApplicationBuilder UseAccessControlHelper(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            // put middleware in pipeline
            return app.UseMiddleware<AccessControlHelperMiddleware>();
        }

        public static IApplicationBuilder UseAccessControlHelper(this IApplicationBuilder app, AccessControlHelperOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            AccessControlAttribute.RegisterAccessStrategy(options.ActionAccessStrategy);
            HtmlHelperExtension.RegisterAccessStrategy(options.ControlAccessStrategy);
            // put middleware in pipeline
            return app.UseMiddleware<AccessControlHelperMiddleware>(Options.Create(options));
        }

        public static IApplicationBuilder UseAccessControlHelper(this IApplicationBuilder app, Action<AccessControlHelperOptions> optionsAction)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (optionsAction == null)
            {
                throw new ArgumentNullException(nameof(optionsAction));
            }
            AccessControlHelperOptions options = new AccessControlHelperOptions();
            optionsAction(options);
            AccessControlAttribute.RegisterAccessStrategy(options.ActionAccessStrategy);
            HtmlHelperExtension.RegisterAccessStrategy(options.ControlAccessStrategy);
            // put middleware in pipeline
            return app.UseMiddleware<AccessControlHelperMiddleware>(Options.Create(options));
        }
#endif
    }
}