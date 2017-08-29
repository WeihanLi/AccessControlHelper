using System;
using System.Threading.Tasks;

#if !NET45

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

#endif

namespace AccessControlHelper
{
#if !NET45

    /// <summary>
    /// AccessControlHelperMiddleware
    /// </summary>
    public class AccessControlHelperMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AccessControlHelperOptions _options;
        private readonly ILogger _logger;

        /// <summary>
        /// Creates a new instance of <see cref="AccessControlHelperMiddleware"/>
        /// </summary>
        /// <param name="next">The delegate representing the next middleware in the request pipeline.</param>
        /// <param name="hostingEnvironment">The Hosting Environment.</param>
        /// <param name="loggerFactory">The Logger Factory.</param>
        /// <param name="options">The middleware options, containing the rules to apply.</param>
        public AccessControlHelperMiddleware(
            RequestDelegate next,
            IHostingEnvironment hostingEnvironment,
            ILoggerFactory loggerFactory,
            IOptions<AccessControlHelperOptions> options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _next = next;
            _options = options.Value;
            _logger = loggerFactory.CreateLogger(typeof(AccessControlHelperMiddleware).FullName);
        }

        /// <summary>
        /// Executes the middleware.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public Task Invoke(HttpContext context)
        {
            return _next(context);
        }
    }

    /// <summary>
    /// AccessControlHelperOption
    /// </summary>
    public class AccessControlHelperOptions
    {
        /// <summary>
        /// Action 访问策略
        /// </summary>
        public IActionAccessStrategy ActionAccessStrategy { get; set; }

        public IControlAccessStrategy ControlAccessStrategy { get; set; }
    }

#endif
}