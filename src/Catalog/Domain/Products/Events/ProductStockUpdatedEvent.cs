using System;
using Webshop.Shared.Ddd;

namespace Catalog.Domain.Products.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductStockUpdatedEvent: IDomainEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
