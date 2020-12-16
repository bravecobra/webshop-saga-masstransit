using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Webshop.Shared.Infrastructure.ErrorHandling
{
    /// <summary>
    ///     Middleware that provides error handling.
    ///     Please make sure this middleware is added first, since it should handle all exceptions that occur in the pipeline (including possible exceptions in other middleware!).
    /// <remarks>
    ///     Should be added first
    /// </remarks>
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly ILookup<Type, IExceptionHandler> _exceptionHandlers;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IWebHostEnvironment env, IEnumerable<IExceptionHandler> exceptionHandlers)
        {
            _next = next;
            _logger = logger;
            _env = env;
            _exceptionHandlers = exceptionHandlers.ToLookup(x => x.ExceptionType);
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            int code = (int)HttpStatusCode.InternalServerError; // 500 if unexpected
            string result;

            List<IExceptionHandler> correspondingHandlers = _exceptionHandlers[ex.GetType()]
                .OrderBy(handler => handler.Order)
                .ToList();

            dynamic info = new System.Dynamic.ExpandoObject();
            foreach (IExceptionHandler exceptionHandler in correspondingHandlers)
            {
                exceptionHandler.Handle(ex, context, info, out code);
            }

            info.traceId = context.TraceIdentifier;
            if (correspondingHandlers.Any())
            {
                _logger.LogWarning(ex, "An exception occured, but it was handled by the following handlers: {@handlers}", correspondingHandlers);
                result = JsonConvert.SerializeObject(info);
            }
            else
            {
                _logger.LogWarning(ex, "An error occured, but no corresponding handler was found!");
                if (_env.IsProduction())
                {
                    result = JsonConvert.SerializeObject(new
                    {
                        error = new { message = "An unknown internal server error occured.", traceId = context.TraceIdentifier }
                    });
                }
                else
                {
                    result = JsonConvert.SerializeObject(new
                    {
                        error = new { message = "An unknown internal server error occured.", detail = ex, traceId = context.TraceIdentifier }
                    });
                }
            }

            _logger.LogError(ex, result);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;
            return context.Response.WriteAsync(result);
        }
    }
}
