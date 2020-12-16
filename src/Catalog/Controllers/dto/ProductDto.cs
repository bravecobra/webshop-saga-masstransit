using System;

namespace Catalog.Controllers.dto
{
    // ReSharper disable UnusedMember.Global
    /// <summary>
    /// Describes a product with Price and Stock information
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// The Id of the product
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// This is the description of the product
        /// </summary>
        public string Description { get; set; } = null!;
        /// <summary>
        /// The current price information of the product
        /// </summary>
        public PriceInfoDto? PriceInfo { get; set; }
        /// <summary>
        /// The current stockinformation of the product
        /// </summary>
        public StockInfoDto? StockInfo { get; set; }
    }
}