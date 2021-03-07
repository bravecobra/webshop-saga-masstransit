using System;
using Microsoft.Extensions.Configuration;

namespace Catalog.Configuration
{
    /// <inheritdoc />
    public class VaultConfigurationSource : IConfigurationSource
    {
        private readonly VaultOptions _config;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public VaultConfigurationSource(Action<VaultOptions> config)
        {
            _config = new VaultOptions();
            config.Invoke(_config);
        }

        /// <inheritdoc />
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new VaultConfigurationProvider(_config);
        }
    }
}