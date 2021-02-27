using System;
using Webshop.Shared.Ddd;

namespace Catalog.Domain.Products.Events
{
    /// <summary>
    ///
    /// </summary>
    public class ProductDescriptionUpdated : IDomainEvent
    {
        /// <summary>
        ///
        /// </summary>
        public Guid ProductId { get; init; }
        /// <summary>
        ///
        /// </summary>
        public string Description { get; init; }  = null!;
    }
}