using System;

namespace Webshop.Shared.Ddd
{
    public abstract class DomainEvent<T, TKey> : IDomainEvent where T : IEntity<TKey>
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public T Entity { get; }

        protected DomainEvent(T entity)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
        }

        public override string ToString()
        {
            return $"{GetType().FullName} for entity '{Entity.Id}'";
        }
    }
}