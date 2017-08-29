using System;
using WeihanLi.AspNetMvc.AccessControlHelper;
#if !NET45

using Microsoft.Extensions.Options;

#endif

namespace Microsoft.AspNetCore.Builder
{
#if !NET45

    public static class AccessControlHelperExtension
    {
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
    }

#endif
}