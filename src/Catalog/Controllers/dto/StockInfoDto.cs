using System;

namespace Catalog.Controllers.dto
{
    // ReSharper disable UnusedMember.Global
    /// <summary>
    /// Stockinformation of a product. This will get updated by the warehouse when stock of the product changes 
    /// </summary>
    public class StockInfoDto
    {
        /// <summary>
        /// The current amount that the warehouse reported to have in stock
        /// </summary>
        public int CurrentAmountAvailable { get; set; }
        /// <summary>
        /// Last time the warehouse reported a stock change for the product 
        /// </summary>
        public DateTimeOffset LastChanged { get; set; }
    }
}