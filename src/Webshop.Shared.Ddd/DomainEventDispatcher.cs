using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Webshop.Shared.Ddd
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DomainEventDispatcher> _logger;

        public DomainEventDispatcher(IMediator mediator, ILogger<DomainEventDispatcher> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task DispatchDomainEvents(params IAggregateRoot[] aggregateRoots)
        {
            foreach (var aggregateRoot in aggregateRoots)
            {
                foreach (var domainEvent in aggregateRoot.DomainEvents)
                {
                    _logger.LogInformation("Dispatching {DomainEvent} of {AggregateRoot}", domainEvent, aggregateRoot);
                    await _mediator.Publish(domainEvent);
                }
            }
        }
    }
}