using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    /// <summary>
    /// AccessControlHelperMiddleware
    /// </summary>
    public class AccessControlHelperMiddleware
    {
        private readonly RequestDelegate _next;
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
            ILogger<AccessControlHelperMiddleware> logger)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }
            _next = next;
            _logger = logger;
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
}