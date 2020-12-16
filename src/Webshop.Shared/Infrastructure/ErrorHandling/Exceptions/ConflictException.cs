using System;

namespace Webshop.Shared.Infrastructure.ErrorHandling.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {

        }
    }
}