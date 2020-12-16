using Catalog.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Persistence.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductMapping: IEntityTypeConfiguration<Product>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products")
                .HasKey(product => product.Id);
            builder.Property(product => product.Description).IsRequired();

            builder.OwnsOne(e => e.PriceInfo, eb =>
            {
                eb.WithOwner()
                    .HasForeignKey(info => info.ProductId);
                eb.ToTable("Prices");
                eb.HasKey(info => info.Id);
                eb.Property(info => info.ProductId).IsRequired();
                eb.Property(info => info.LastChanged).IsRequired();
                eb.OwnsOne(p => p.LatestPrice);
            });

            builder.OwnsOne(e => e.StockInfo, eb =>
            {
                eb.WithOwner()
                    .HasForeignKey(info => info.ProductId);
                eb.ToTable("StockInfo");
                eb.HasKey(info => info.Id);
                eb.Property(info => info.CurrentAmountAvailable).IsRequired();
                eb.Property(info => info.LastChanged).IsRequired();
                eb.Property(info => info.ProductId).IsRequired();
            });

            builder.Property(p => p.Description);
        }
    }
}
