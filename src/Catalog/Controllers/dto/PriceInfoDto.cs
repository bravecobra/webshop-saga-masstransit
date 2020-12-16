using System;

namespace Catalog.Controllers.dto
{
    // ReSharper disable UnusedMember.Global
    /// <summary>
    /// The latest price of a product. This will get updated by the pricing service when prices changes over time
    /// </summary>
    public class PriceInfoDto
    {
        /// <summary>
        /// The current price 
        /// </summary>
        public PriceDto LatestPrice { get; set; } = null!;
        /// <summary>
        /// Last time the price was updated
        /// </summary>
        public DateTimeOffset LastChanged { get; set; }
    }
}