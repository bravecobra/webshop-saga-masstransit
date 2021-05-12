using AutoMapper;
using Catalog.Domain.Products;
using Catalog.Domain.Products.PriceInfo;
using Catalog.Domain.Products.StockInfo;

namespace Catalog.Controllers.dto
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductsProfile: Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public ProductsProfile()
        {
            CreateMap<Product, ProductDto>()
                .ReverseMap();
            CreateMap<PriceInfo, PriceInfoDto>().ReverseMap();
            CreateMap<Price, PriceDto>().ReverseMap();
            CreateMap<StockInfo, StockInfoDto>().ReverseMap();
        }
    }
}
