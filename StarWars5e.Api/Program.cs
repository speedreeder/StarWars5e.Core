using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;

namespace StarWars5e.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                        {
                            // Set properties and call methods on options
                        })
                        .UseStartup<Startup>();
                });

        //WebHost.CreateDefaultBuilder(args)
        //    .UseApplicationInsights()
        //    .ConfigureAppConfiguration((context, config) =>
        //    {
        //        if (context.HostingEnvironment.IsProduction())
        //        {
        //            var builtConfig = config.Build();

        //            var azureServiceTokenProvider = new AzureServiceTokenProvider();
        //            var keyVaultClient = new KeyVaultClient(
        //                new KeyVaultClient.AuthenticationCallback(
        //                    azureServiceTokenProvider.KeyVaultTokenCallback));

        //            config.AddAzureKeyVault(
        //                $"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
        //                keyVaultClient,
        //                new DefaultKeyVaultSecretManager());                       
        //        }
        //    })
        //    .UseStartup<Startup>();
    }
}
