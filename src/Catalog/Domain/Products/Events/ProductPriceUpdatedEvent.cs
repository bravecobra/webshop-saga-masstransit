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
        public Guid ProductId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Price NewPrice { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
