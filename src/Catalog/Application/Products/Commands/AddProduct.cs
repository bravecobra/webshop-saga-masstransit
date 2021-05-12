using System;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Products;
using Catalog.Persistence;
using FluentValidation;
using MediatR;
using Webshop.Shared.Ddd;

namespace Catalog.Application.Products.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class AddProduct : IRequestHandler<AddProduct.AddProductRequest, Guid>
    {
        private readonly CatalogDbContext _context;
        private readonly DomainEventDispatcher _dispatcher;
        private readonly IProductRepository _repository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dispatcher"></param>
        /// <param name="repository"></param>
        public AddProduct(CatalogDbContext context, DomainEventDispatcher dispatcher, IProductRepository repository)
        {
            _context = context;
            _dispatcher = dispatcher;
            _repository = repository;
        }

        /// <summary>
        /// 
        /// </summary>
        public class AddProductRequest : IRequest<Guid>
        {
            /// <summary>
            /// 
            /// </summary>
            public Product Product { get; set; } = null!;
        }


        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public class AddProductValidator : AbstractValidator<AddProductRequest>
        {
            /// <summary>
            /// 
            /// </summary>
            public AddProductValidator()
            {
                RuleFor(product => product.Product.Description).NotEmpty().WithMessage("Description should not be empty");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Guid> Handle(AddProductRequest request, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var product = Product.CreateProduct(request.Product.Description, 
                    request.Product.StockInfo.CurrentAmountAvailable, 
                    request.Product.PriceInfo.LatestPrice);
                await _repository.AddProduct(product, cancellationToken);
                await _dispatcher.DispatchDomainEvents(product);
                await transaction.CommitAsync(cancellationToken);
                //todo: publish application event product added
                return product.Id;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
