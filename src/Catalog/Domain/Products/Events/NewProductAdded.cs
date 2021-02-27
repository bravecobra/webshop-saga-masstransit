using System;
using Webshop.Shared.Ddd;

namespace Catalog.Domain.Products.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class NewProductAdded: IDomainEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid ProductId { get; init; }
    }
}
