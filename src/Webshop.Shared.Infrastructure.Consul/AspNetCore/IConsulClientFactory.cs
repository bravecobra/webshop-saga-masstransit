using Consul;

namespace Webshop.Shared.Infrastructure.Consul.AspNetCore
{
    public interface IConsulClientFactory
    {
        IConsulClient CreateClient();
        IConsulClient CreateClient(string name);
    }
}