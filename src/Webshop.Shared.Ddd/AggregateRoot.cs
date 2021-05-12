using System.Collections.Generic;

namespace Webshop.Shared.Ddd
{
    public abstract class AggregateRoot<T> : IAggregateRoot, IEntity<T>
    {
        public T Id { get; set; }

        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}