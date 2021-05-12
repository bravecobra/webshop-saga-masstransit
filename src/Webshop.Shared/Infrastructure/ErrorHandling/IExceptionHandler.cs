using System;
using Microsoft.AspNetCore.Http;

namespace Webshop.Shared.Infrastructure.ErrorHandling
{
    public interface IExceptionHandler
    {
        /// <summary>
        /// Gets the order in which this exception handler will be executed.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Gets the type of the <see cref="Exception"/> for which the exception handler is meant.
        /// </summary>
        Type ExceptionType { get; }

        /// <summary>
        ///     Will handle the thrown exception.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> that is thrown.</param>
        /// <param name="httpContext">The <see cref="HttpContext"/> object belonging to the current request.</param>
        /// <param name="resultObject">A dynamic object to which additional resultObject can be specified. This will be serialized as a Json object and included in the response.</param>
        /// <param name="returnCode">The return code that should </param>
        void Handle(Exception ex, HttpContext httpContext, dynamic resultObject, out int returnCode);
    }
}