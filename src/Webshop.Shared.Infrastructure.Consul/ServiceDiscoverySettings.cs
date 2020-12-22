namespace Webshop.Shared.Infrastructure.Consul
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceDiscoverySettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string DataCenter { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int WaitTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ServiceName { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool SelfRegistration { get; set; } = false;
    }
}