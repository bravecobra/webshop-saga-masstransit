using Catalog.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Catalog
{
    /// <summary>
    /// 
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {

                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables(prefix: "VAULT_");

                    var builtConfig = config.Build();

                    if (builtConfig.GetSection("Vault")["Role"] != null)
                    {
                        config.AddVault(options =>
                        {
                            var vaultOptions = builtConfig.GetSection("Vault");
                            options.Address = vaultOptions["Address"];
                            options.Role = vaultOptions["Role"];
                            options.MountPath = vaultOptions["MountPath"];
                            options.SecretType = vaultOptions["SecretType"];
                            options.Secret = builtConfig.GetSection("VAULT_SECRET_ID").Value;
                        });
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
