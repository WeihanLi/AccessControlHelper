using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    /// <summary>
    /// AccessControlHelperMiddleware
    /// </summary>
    public class AccessControlHelperMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IResourceAccessStrategy _accessStrategy;
        private readonly ILogger _logger;
        private readonly AccessControlOption _option;

        /// <summary>
        /// Creates a new instance of <see cref="AccessControlHelperMiddleware"/>
        /// </summary>
        /// <param name="next">The delegate representing the next middleware in the request pipeline.</param>
        /// <param name="options"></param>
        /// <param name="logger">The Logger Factory.</param>
        /// <param name="accessStrategy">actionAccessStrategy</param>
        public AccessControlHelperMiddleware(
            RequestDelegate next,
            IOptions<AccessControlOption> options,
            ILogger<AccessControlHelperMiddleware> logger, IResourceAccessStrategy accessStrategy)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
            _accessStrategy = accessStrategy;
            _option = options.Value;
        }

        /// <summary>
        /// Executes the middleware.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public Task Invoke(HttpContext context)
        {
            // add custom operation
            var accessKey = string.Empty;
            if (context.Request.Headers.ContainsKey(_option.AccessHeaderKey))
            {
                accessKey = context.Request.Headers[_option.AccessHeaderKey].ToString();
            }
            if (_accessStrategy.IsCanAccess(accessKey))
            {
                return _next(context);
            }
            //
            _logger.LogInformation($"Request {context.TraceIdentifier} was unauthorized, Request path:{context.Request.Path}");

            context.Response.StatusCode = 403;

            _option.DefaultUnauthorizedOperation?.Invoke(context.Response);

            return Task.CompletedTask;
        }
    }
}
