using System.Net;
using Microsoft.AspNetCore.Http;

namespace Webshop.Shared.Infrastructure.ErrorHandling.Exceptions
{
    public class ConflictExceptionHandler : ExceptionHandler<ConflictException>
    {
        protected override void Handle(ConflictException ex, HttpContext httpContext, dynamic info, out int returnCode)
        {
            returnCode = (int)HttpStatusCode.Conflict;
            info.error = ex.Message;
        }
    }
}