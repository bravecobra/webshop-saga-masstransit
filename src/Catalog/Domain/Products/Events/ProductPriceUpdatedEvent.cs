using System;
using Catalog.Domain.Products.PriceInfo;
using Webshop.Shared.Ddd;

namespace Catalog.Domain.Products.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductPriceUpdatedEvent: IDomainEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid ProductId { get; init; }
        /// <summary>
        /// 
        /// </summary>
        public Price NewPrice { get; init; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset UpdatedOn { get; init; }
    }
}
