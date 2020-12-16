using System;
using Microsoft.AspNetCore.Http;

namespace Webshop.Shared.Infrastructure.ErrorHandling
{
    public abstract class ExceptionHandler<TException> : IExceptionHandler
        where TException : Exception
    {
        public int Order { get; } = 0;
        public Type ExceptionType { get; } = typeof(TException);

        public void Handle(Exception ex, HttpContext httpContext, dynamic resultObject, out int returnCode)
        {
            Handle(ex as TException, httpContext, resultObject, out returnCode);
        }

        protected abstract void Handle(TException ex, HttpContext httpContext, dynamic resultObject, out int returnCode);
    }
}