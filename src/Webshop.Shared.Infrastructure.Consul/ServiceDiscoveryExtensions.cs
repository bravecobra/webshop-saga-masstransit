using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Webshop.Shared.Infrastructure.Consul.AspNetCore;

namespace Webshop.Shared.Infrastructure.Consul
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceDiscoveryExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsulSelfRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IConsulClient, ConsulClient>();
            var settings = new ServiceDiscoverySettings();
            configuration.GetSection("ServiceDiscovery").Bind(settings);
            if (!settings.SelfRegistration) return services;

            var hostName = Dns.GetHostName(); //returns the Docker ID
            var ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList
                .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork)?.ToString();
            services.AddConsulServiceRegistration(options =>
            {
                options.ID = hostName;
                options.Name = settings.ServiceName;
                options.EnableTagOverride = settings.Tags?.Length > 1;
                options.Address = ip;
                options.Tags = settings.Tags;
                options.Port = settings.Port;
                options.Checks = new[]
                {
                    //This check lets Consul verify connectivity using the health enpoint at /health
                    new AgentServiceCheck
                    {
                        HTTP = $"http://{ip}:{settings.Port}/health",
                        Interval = TimeSpan.FromSeconds(10)
                    }
                };
            });
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsulClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IConsulClient, ConsulClient>();
            var settings = new ServiceDiscoverySettings();
            configuration.GetSection("ServiceDiscovery").Bind(settings);
            if (!settings.Enabled) return services;

            var hostName = Dns.GetHostName();
            var ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList
                .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork)?.ToString();
            services.AddConsul(options =>
            {
                options.Datacenter = settings.DataCenter;
                options.Address = new Uri(settings.Address);
                options.Token = settings.Token;
                options.WaitTime = TimeSpan.FromMilliseconds(settings.WaitTime);
            });
            return services;
        }
    }
}
