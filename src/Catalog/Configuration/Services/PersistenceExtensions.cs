using Catalog.Persistence;
using Microsoft.Data.SqlClient;
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
            var dbBuilder = new SqlConnectionStringBuilder(
                configuration.GetConnectionString("Database")
            );

            if (configuration["database:userID"] != null)
            {
                dbBuilder.UserID = configuration["database:userID"];
                dbBuilder.Password = configuration["database:password"];

                configuration.GetSection("ConnectionStrings")["Database"] = dbBuilder.ConnectionString;
            }

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
