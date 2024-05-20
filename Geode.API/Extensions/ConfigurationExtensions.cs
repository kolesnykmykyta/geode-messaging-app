using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Geode.API.Extensions
{
    internal static class ConfigurationExtensions
    {
        internal static void ConfigureAzureKeyVault(this WebApplicationBuilder builder)
        {
            string keyVaultUrl = builder.Configuration["KeyVaultConfig:KeyVaultUrl"]!;
            string tenantId = builder.Configuration["KeyVaultConfig:TenantId"]!;
            string clientId = builder.Configuration["KeyVaultConfig:ClientId"]!;
            string clientSecret = builder.Configuration["KeyVaultConfig:ClientSecret"]!;

            ClientSecretCredential credentials = new ClientSecretCredential(tenantId, clientId, clientSecret);

            SecretClient secretClient = new SecretClient(new Uri(keyVaultUrl), credentials);
            builder.Configuration.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
        }
    }
}
