using System.Collections.Generic;
using Webshop.Shared.Ddd;

namespace Catalog.Domain.Products.PriceInfo
{
    /// <summary>
    /// 
    /// </summary>
    public class Price: ValueObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        public Price(float amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }
        /// <summary>
        /// 
        /// </summary>
        public float Amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Currency} {Amount}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Price NullPrice()
        {
            return new Price(0, "USD");
        }
    }
}
