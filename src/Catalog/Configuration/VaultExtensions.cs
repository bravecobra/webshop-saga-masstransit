using System;
using Microsoft.Extensions.Configuration;

namespace Catalog.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class VaultExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddVault(this IConfigurationBuilder configuration,
            Action<VaultOptions> options)
        {
            var vaultOptions = new VaultConfigurationSource(options);
            configuration.Add(vaultOptions);
            return configuration;
        }
    }
}