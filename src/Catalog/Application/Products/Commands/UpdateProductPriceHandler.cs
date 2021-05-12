using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Products;
using Catalog.Domain.Products.PriceInfo;
using Catalog.Persistence;
using FluentValidation;
using MediatR;
using Webshop.Shared.Ddd;
using Webshop.Shared.Infrastructure.ErrorHandling.Exceptions;

namespace Catalog.Application.Products.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateProductPrice : IRequestHandler<UpdateProductPrice.UpdateProductPriceCommand>
    {
        private readonly DomainEventDispatcher _dispatcher;
        private readonly IProductRepository _repository;
        private readonly Product.ProductValidator _validator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="repository"></param>
        /// <param name="validator"></param>
        public UpdateProductPrice(DomainEventDispatcher dispatcher, IProductRepository repository, Product.ProductValidator validator)
        {
            _dispatcher = dispatcher;
            _repository = repository;
            _validator = validator;
        }
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once ClassNeverInstantiated.Global
        public class UpdateProductPriceCommand : IRequest<Unit>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="productId"></param>
            /// <param name="price"></param>
            public UpdateProductPriceCommand(Guid productId, Price price)
            {
                ProductId = productId;
                Price = price;
            }

            /// <summary>
            /// 
            /// </summary>
            public Guid ProductId { get; }

            /// <summary>
            /// 
            /// </summary>
            public Price Price { get; set; } = null!;
        }

        /// <summary>
        /// 
        /// </summary>
        public class UpdateProductPriceValidator : AbstractValidator<UpdateProductPriceCommand>
        {
            /// <summary>
            ///
            /// </summary>
            public UpdateProductPriceValidator()
            {
                RuleFor(x => x.ProductId).NotEmpty().WithMessage("The productId should be set");
                RuleFor(x => x.Price).NotNull().WithMessage("PriceInfo should be set");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Unit> Handle(UpdateProductPriceCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _repository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Get the original product
                var productToUpdate = await _repository.GetProductById(request.ProductId, cancellationToken);
                if (productToUpdate == null)
                    throw new NotFoundException($"No product was found with id {request.ProductId}");
                productToUpdate.UpdatePriceInfo(request.Price);
                await _validator.ValidateAndThrowAsync(productToUpdate, cancellationToken);
                await _repository.SaveProduct(cancellationToken);
                await _dispatcher.DispatchDomainEvents(productToUpdate);
                await transaction.CommitAsync(cancellationToken);
                return default;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
