using System;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Products;
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
    public class UpdateProduct: IRequestHandler<UpdateProduct.UpdateProductCommand>
    {
        private readonly Product.ProductValidator _validator;
        private readonly IProductRepository _repository;
        private readonly DomainEventDispatcher _dispatcher;

        /// <summary>
        ///
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="repository"></param>
        /// <param name="dispatcher"></param>
        public UpdateProduct(Product.ProductValidator validator, IProductRepository repository, DomainEventDispatcher dispatcher)
        {
            _validator = validator;
            _repository = repository;
            _dispatcher = dispatcher;
        }
        /// <summary>
        ///
        /// </summary>
        public class UpdateProductCommand : IRequest<Unit>
        {
            /// <summary>
            ///
            /// </summary>
            /// <param name="id"></param>
            public UpdateProductCommand(Guid id)
            {
                Id = id;
            }

            /// <summary>
            ///
            /// </summary>
            public Guid Id { get; set; }

            /// <summary>
            ///
            /// </summary>
            public Product Product { get; set; } = null!;
        }

        /// <summary>
        ///
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
        {
            /// <summary>
            ///
            /// </summary>
            /// <param name="productValidator"></param>
            public UpdateProductValidator(IValidator<Product> productValidator)
            {
                RuleFor(x => x.Product).SetValidator(productValidator);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="updateProductCommand"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Unit> Handle(UpdateProductCommand updateProductCommand, CancellationToken cancellationToken)
        {
            await using var transaction = await _repository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Get the original product
                var productToUpdate = await _repository.GetProductById(updateProductCommand.Id, cancellationToken);
                if (productToUpdate == null)
                    throw new NotFoundException($"No product was found with id {updateProductCommand.Id}");
                // update the values
                productToUpdate.UpdateDescription(updateProductCommand.Product.Description);
                //productToUpdate.UpdatePriceInfo(updateProductCommand.Product.PriceInfo.Price);
                //productToUpdate.UpdateStockInfo(updateProductCommand.Product.StockInfo.AmountAvailable);
                await _validator.ValidateAndThrowAsync(productToUpdate, cancellationToken);
                await _repository.SaveProduct(cancellationToken);
                await _dispatcher.DispatchDomainEvents(productToUpdate);
                //todo: publish application event
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
