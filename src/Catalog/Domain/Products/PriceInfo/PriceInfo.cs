using System;
using Webshop.Shared.Ddd;

namespace Catalog.Domain.Products.PriceInfo
{
    /// <summary>
    /// 
    /// </summary>
    public class PriceInfo: IEntity<Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected PriceInfo()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public Price LatestPrice { get; private set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset LastChanged { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="price"></param>
        public void UpdatePrice(Price price)
        {
            if (Equals(price, LatestPrice)) return;
            LatestPrice = price;
            LastChanged = DateTimeOffset.UtcNow;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static PriceInfo CreatePriceInfo(Price price)
        {
            return new PriceInfo
            {
                Id = Guid.NewGuid(),
                LatestPrice = price,
                LastChanged = DateTimeOffset.UtcNow
            };
        }
    }
}