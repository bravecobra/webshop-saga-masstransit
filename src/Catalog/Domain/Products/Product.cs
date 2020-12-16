using System;
using Catalog.Domain.Products.Events;
using Catalog.Domain.Products.PriceInfo;
using FluentValidation;
using Webshop.Shared.Ddd;

namespace Catalog.Domain.Products
{
    /// <summary>
    /// 
    /// </summary>
    public class Product: AggregateRoot<Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        protected Product() {
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; private set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        public void UpdateDescription(string description)
        {
            if (Description == description) return;
            Description = description;
            RaiseDomainEvent(new ProductDescriptionUpdated
            {
                ProductId = this.Id,
                Description = Description
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual PriceInfo.PriceInfo PriceInfo { get; private set; } = Products.PriceInfo.PriceInfo.CreatePriceInfo(Price.NullPrice());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="price"></param>
        public void UpdatePriceInfo(Price? price)
        {
            PriceInfo.UpdatePrice(price ?? Price.NullPrice());
            RaiseDomainEvent(new ProductPriceUpdatedEvent
            {
                ProductId = this.Id,
                NewPrice = PriceInfo.LatestPrice,
                UpdatedOn = PriceInfo.LastChanged
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual StockInfo.StockInfo StockInfo { get; private set; } = Products.StockInfo.StockInfo.CreateStockInfo();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        public void UpdateStockInfo(int? amount)
        {
            StockInfo.UpdateAmountAvailable(amount);
            RaiseDomainEvent(new ProductStockUpdatedEvent
            {
                ProductId = Id,
                Amount = StockInfo.CurrentAmountAvailable,
                UpdatedOn = StockInfo.LastChanged
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="amountAvailable"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public static Product CreateProduct(string description, int? amountAvailable = 0, Price? price = null)
        {
            var product = new Product
            {
                Id = Guid.NewGuid()
            };
            product.RaiseDomainEvent(new NewProductAdded
            {
                ProductId = product.Id
            });
            product.UpdateDescription(description);
            product.UpdatePriceInfo(price);
            product.UpdateStockInfo(amountAvailable);
            return product;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        public static void DeleteProduct(Product product)
        {
            product.RaiseDomainEvent(new ProductDeleted
            {
                ProductId = product.Id
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public class ProductValidator : AbstractValidator<Product>
        {
            /// <summary>
            /// 
            /// </summary>
            public ProductValidator()
            {
                RuleFor(product => product.Description).NotEmpty().WithMessage("Description should not be empty");
                RuleFor(product => product.StockInfo).NotNull().WithMessage("A product should have stock information");
                RuleFor(product => product.PriceInfo).NotNull().WithMessage("A product should have price information");
            }
        }
    }
}
