using System;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Products;
using Catalog.Persistence;
using MediatR;

namespace Catalog.Application.Products.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdHandler.GetProductByIdQuery, Product?>
    {
        private readonly IProductRepository _repository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public GetProductByIdHandler(IProductRepository repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetProductById(request.Id, cancellationToken);
        }
        /// <summary>
        /// 
        /// </summary>
        public class GetProductByIdQuery : IRequest<Product?>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            public GetProductByIdQuery(Guid id)
            {
                Id = id;
            }
            /// <summary>
            /// 
            /// </summary>
            public Guid Id { get; set; }
        }
    }
}
