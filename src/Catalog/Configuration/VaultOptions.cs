namespace Catalog.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class VaultOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        public string Role { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public string Secret { get; set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        public string MountPath { get; set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        public string SecretType { get; set; } = null!;
    }
}