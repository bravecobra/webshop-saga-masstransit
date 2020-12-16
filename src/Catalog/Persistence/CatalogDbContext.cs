using Catalog.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence
{
    /// <summary>
    /// 
    /// </summary>
    public class CatalogDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options): base(options)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Product> Products { get; set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(false);
            optionsBuilder.EnableDetailedErrors(false);
            base.OnConfiguring(optionsBuilder);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        }
    }
}
