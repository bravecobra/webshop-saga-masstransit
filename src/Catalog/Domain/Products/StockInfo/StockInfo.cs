using System;
using Webshop.Shared.Ddd;

namespace Catalog.Domain.Products.StockInfo
{
    /// <summary>
    /// 
    /// </summary>
    public class StockInfo: IEntity<Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected StockInfo() { }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentAmountAvailable { get; private set; }

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
        /// <param name="currentAmountAvaliable"></param>
        protected StockInfo(int currentAmountAvaliable)
        {
            CurrentAmountAvailable = currentAmountAvaliable;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsAvailable()
        {
            return CurrentAmountAvailable > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        public void UpdateAmountAvailable(int? amount)
        {
            if (amount == CurrentAmountAvailable) return;
            CurrentAmountAvailable = amount ?? 0;
            LastChanged = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amountAvailable"></param>
        /// <returns></returns>
        public static StockInfo CreateStockInfo(int? amountAvailable = 0)
        {
            var stockinfo = new StockInfo
            {
                Id = Guid.NewGuid(),
                CurrentAmountAvailable = amountAvailable ?? 0
            };
            return stockinfo;
        }
    }
}