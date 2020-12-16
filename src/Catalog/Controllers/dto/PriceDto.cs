namespace Catalog.Controllers.dto
{
    // ReSharper disable UnusedMember.Global
    /// <summary>
    /// Defines a price
    /// </summary>
    public class PriceDto
    {
        /// <summary>
        /// The amount of the price
        /// </summary>
        public float Amount { get; set; }

        /// <summary>
        /// The currency of the price
        /// </summary>
        public string Currency { get; set; } = null!;
    }
}