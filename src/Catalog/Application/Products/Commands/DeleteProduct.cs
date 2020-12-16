using System;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Persistence;
using MediatR;
using Webshop.Shared.Ddd;

namespace Catalog.Application.Products.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteProduct : IRequestHandler<DeleteProduct.DeleteProductRequest>
    {
        private readonly IProductRepository _repository;
        private readonly DomainEventDispatcher _dispatcher;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dispatcher"></param>
        public DeleteProduct(IProductRepository repository, DomainEventDispatcher dispatcher)
        {
            _repository = repository;
            _dispatcher = dispatcher;
        }
        /// <summary>
        /// 
        /// </summary>
        public class DeleteProductRequest : IRequest<Unit>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            public DeleteProductRequest(Guid id)
            {
                Id = id;
            }
            /// <summary>
            /// 
            /// </summary>
            public Guid Id { get; private set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Unit> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetProductById(request.Id, cancellationToken);
            await _repository.DeleteProduct(request.Id, cancellationToken);
            await _dispatcher.DispatchDomainEvents(product);
            //todo: publish application event product added
            return default;
        }
    }
}