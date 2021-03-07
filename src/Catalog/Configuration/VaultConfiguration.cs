using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using VaultSharp;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines;

namespace Catalog.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class VaultConfigurationProvider : ConfigurationProvider
    {
        private readonly VaultOptions _config;
        private readonly IVaultClient _client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public VaultConfigurationProvider(VaultOptions config)
        {
            _config = config;

            var vaultClientSettings = new VaultClientSettings(
                _config.Address,
                new AppRoleAuthMethodInfo(_config.Role,
                                          _config.Secret)
            );

            _client = new VaultClient(vaultClientSettings);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Load()
        {
            LoadAsync().Wait();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task LoadAsync()
        {
            await GetDatabaseCredentials();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetDatabaseCredentials()
        {
            var userID = "";
            var password = "";

            if (_config.SecretType == "secrets")
            {
                var secrets = await _client.V1.Secrets.KeyValue.V2.ReadSecretAsync(
                  "static", null, _config.MountPath + _config.SecretType);

                userID = "sa";
                password = secrets.Data.Data["password"].ToString();
            }

            if (_config.SecretType == "database")
            {
                var dynamicDatabaseCredentials =
                await _client.V1.Secrets.Database.GetCredentialsAsync(
                  _config.Role,
                  _config.MountPath + _config.SecretType);

                userID = dynamicDatabaseCredentials.Data.Username;
                password = dynamicDatabaseCredentials.Data.Password;
            }

            Data.Add("database:userID", userID);
            Data.Add("database:password", password);
        }
    }
}