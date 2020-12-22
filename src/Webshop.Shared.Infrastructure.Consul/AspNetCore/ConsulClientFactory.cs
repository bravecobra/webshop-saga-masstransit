﻿using Consul;
using Microsoft.Extensions.Options;

namespace Webshop.Shared.Infrastructure.Consul.AspNetCore
{
    public class ConsulClientFactory : IConsulClientFactory
    {
        private readonly IOptionsMonitor<ConsulClientConfiguration> _optionsMonitor;

        public ConsulClientFactory(IOptionsMonitor<ConsulClientConfiguration> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;
        }

        public IConsulClient CreateClient()
        {
            return CreateClient(Options.DefaultName);
        }

        public IConsulClient CreateClient(string name)
        {
            var options = _optionsMonitor.Get(name);

            return new ConsulClient(options);
        }
    }
}