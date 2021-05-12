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
    // ReSharper disable once ClassNeverInstantiated.Global
    public class UpdateProductAvailability: IRequestHandler<UpdateProductAvailability.UpdateProductAvailabilityCommand>
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
        public UpdateProductAvailability(DomainEventDispatcher dispatcher, IProductRepository repository, Product.ProductValidator validator)
        {
            _dispatcher = dispatcher;
            _repository = repository;
            _validator = validator;
        }
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once ClassNeverInstantiated.Global
        public class UpdateProductAvailabilityCommand : IRequest<Unit>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="productId"></param>
            /// <param name="amount"></param>
            public UpdateProductAvailabilityCommand(Guid productId, int amount)
            {
                ProductId = productId;
                Amount = amount;
            }

            /// <summary>
            /// 
            /// </summary>
            public Guid ProductId { get; }
            /// <summary>
            /// 
            /// </summary>
            public int Amount { get; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class UpdateProductAvailabilityValidator : AbstractValidator<UpdateProductAvailabilityCommand>
        {
            /// <summary>
            ///
            /// </summary>
            public UpdateProductAvailabilityValidator()
            {
                RuleFor(x => x.ProductId).NotEmpty().WithMessage("The productId should be set");
                RuleFor(x => x.Amount).NotNull().WithMessage("Stockinfo should be set");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Unit> Handle(UpdateProductAvailabilityCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _repository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Get the original product
                var productToUpdate = await _repository.GetProductById(request.ProductId, cancellationToken);
                if (productToUpdate == null)
                    throw new NotFoundException($"No product was found with id {request.ProductId}");
                productToUpdate.UpdateStockInfo(request.Amount);
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
