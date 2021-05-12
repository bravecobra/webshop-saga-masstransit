using System.Collections.Generic;

namespace Webshop.Shared.Ddd
{
    public interface IAggregateRoot
    {
        IReadOnlyList<IDomainEvent> DomainEvents { get; }
    }
}