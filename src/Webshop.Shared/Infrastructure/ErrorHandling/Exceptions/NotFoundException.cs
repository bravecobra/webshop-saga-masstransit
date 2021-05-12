using System;

namespace Webshop.Shared.Infrastructure.ErrorHandling.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {

        }

        public NotFoundException(string name, object key)
            : base($"\"{name}\" ({key}) was not found.")
        {
        }
    }
}