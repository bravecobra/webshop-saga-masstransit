using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Webshop.Shared.Infrastructure.ErrorHandling
{
    public class ValidationExceptionHandler : ExceptionHandler<ValidationException>
    {
        protected override void Handle(ValidationException ex, HttpContext httpContext, dynamic info, out int returnCode)
        {
            returnCode = (int)HttpStatusCode.BadRequest;
            info.validationErrors = ex.Errors;
        }
    }
}