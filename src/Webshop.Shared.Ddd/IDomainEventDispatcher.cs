using System.Threading.Tasks;

namespace Webshop.Shared.Ddd
{
    public interface IDomainEventDispatcher
    {
        Task DispatchDomainEvents(params IAggregateRoot[] aggregateRoots);
    }
}