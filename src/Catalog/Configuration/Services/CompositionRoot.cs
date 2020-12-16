using Catalog.Persistence;
using Catalog.Persistence.Impl;
using Microsoft.Extensions.DependencyInjection;
using Webshop.Shared.Ddd;

namespace Catalog.Configuration.Services
{
    /// <summary>
    /// 
    /// </summary>
    public static class CompositionRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCompositionRoot(this IServiceCollection services)
        {
            services.AddTransient<DomainEventDispatcher>();
            services.AddTransient<IProductRepository, ProductRepository>();
            return services;
        }
    }
}
