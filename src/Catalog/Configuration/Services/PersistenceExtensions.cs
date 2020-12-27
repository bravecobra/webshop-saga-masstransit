using Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Configuration.Services
{
    /// <summary>
    /// 
    /// </summary>
    public static class PersistenceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogDbContext>(options =>
            {
                options
                    .EnableSensitiveDataLogging(false)
                    .EnableDetailedErrors(false)
                    .UseSqlServer(configuration.GetConnectionString("CatalogDbContext"))
                    .EnableDetailedErrors(false);
            });
            return services;
        }
    }
}
