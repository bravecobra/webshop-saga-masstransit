using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Catalog.Persistence.Impl
{
    internal class ProductRepository : IProductRepository
    {
        private readonly CatalogDbContext _context;

        public ProductRepository(CatalogDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetProducts(CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(product => product.PriceInfo)
                .Include(product => product.StockInfo)
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<Product?> GetProductById(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(product => product.PriceInfo)
                .Include(product => product.StockInfo)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddProduct(Product product, CancellationToken cancellationToken)
        {
            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteProduct(Guid id, CancellationToken cancellationToken)
        {
            var product = await _context.Products.Where(p => p.Id == id).FirstAsync(cancellationToken);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task SaveProduct(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return _context.Database.BeginTransactionAsync(cancellationToken);
        }
    }
}