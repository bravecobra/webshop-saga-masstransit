using System.Net;
using Microsoft.AspNetCore.Http;

namespace Webshop.Shared.Infrastructure.ErrorHandling.Exceptions
{
    public class NotFoundExceptionHandler : ExceptionHandler<NotFoundException>
    {
        protected override void Handle(NotFoundException ex, HttpContext httpContext, dynamic info, out int returnCode)
        {
            returnCode = (int)HttpStatusCode.NotFound;
            info.error = ex.Message;
        }
    }
}