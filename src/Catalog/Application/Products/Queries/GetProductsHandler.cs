using System.Collections.Generic;
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
    public class GetProductsHandler: IRequestHandler<GetProductsHandler.GetProducts, List<Product>>
    {
        private readonly IProductRepository _repository;

        /// <summary>
        /// 
        /// </summary>
        public class GetProducts : IRequest<List<Product>>
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public GetProductsHandler(IProductRepository repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<Product>> Handle(GetProducts request, CancellationToken cancellationToken)
        {
            return await _repository.GetProducts(cancellationToken);
        }
    }
}