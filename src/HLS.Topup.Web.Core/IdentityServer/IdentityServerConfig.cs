using System.Collections.Generic;
using System.Linq;
using HLS.Topup.Dtos.Authentication;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using ServiceStack;

namespace HLS.Topup.Web.IdentityServer
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("default-api", "Default (all) API")
                {
                    Description = "AllFunctionalityYouHaveInTheApplication",
                    ApiSecrets = { new Secret("secret") },
                    Scopes = new List<string>()
                    {
                        "default-api"
                    },
                },
                new("hls-orders", "Orders Service"),
                new("hls-basket", "Basket Service"),
                new("hls-app", "Mobile App"),
                new("hls-web", "Web App"),
                new("hls-payment", "Payment Service"),
                new("hls-backend", "Web backend"),
                new("hls-gmobile", "Gmobile Service"),
                new("hls-events", "Events Service"),
                new("hls-report", "Reporting Service"),
                new("hls-commission", "Commission Service"),
                new("hls-webhook", "Webhooks registration Service")
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new()
                {
                    Name = "user.accountCode",
                    DisplayName = "accountCode",
                    Required = true,
                    UserClaims = new[] { "account_code" },
                }
            };
        }

        public static IEnumerable<Client> GetClients(IConfigurationRoot configuration)
        {
            var clients = new List<Client>();

            foreach (var child in configuration.GetSection("IdentityServer:Clients").GetChildren())
            {
                clients.Add(new Client
                {
                    ClientId = child["ClientId"],
                    ClientName = child["ClientName"],
                    AllowedGrantTypes = child.GetSection("AllowedGrantTypes").GetChildren().Select(c => c.Value)
                        .ToArray(),
                    RequireConsent = bool.Parse(child["RequireConsent"] ?? "false"),
                    AllowOfflineAccess = bool.Parse(child["AllowOfflineAccess"] ?? "false"),
                    ClientSecrets = child.GetSection("ClientSecrets").GetChildren()
                        .Select(secret => new Secret(secret["Value"].Sha256())).ToArray(),
                    AllowedScopes = child.GetSection("AllowedScopes").GetChildren().Select(c => c.Value).ToArray(),
                    RedirectUris = child.GetSection("RedirectUris").GetChildren().Select(c => c.Value).ToArray(),
                    PostLogoutRedirectUris = child.GetSection("PostLogoutRedirectUris").GetChildren()
                        .Select(c => c.Value).ToArray(),
                });
            }

            return clients;
        }

        public static IEnumerable<Client> GetClients(IEnumerable<IdentityServerStorageDto> clientIds)
        {
            var items= clientIds.Select(child => new Client
                {
                    ClientId = child.ClientId,
                    ClientName = child.ClientName,
                    AllowedGrantTypes = child.AllowedGrantTypes?.FromJson<List<string>>(),
                    RequireConsent = child.RequireConsent,
                    AllowOfflineAccess = child.AllowOfflineAccess,
                    ClientSecrets = child.ClientSecrets?.FromJson<List<string>>().Select(secret => new Secret(secret.Sha256())).ToArray(),
                    AllowedScopes = child.AllowedScopes?.FromJson<List<string>>(),
                    RedirectUris = child.RedirectUris?.FromJson<List<string>>(),
                    PostLogoutRedirectUris = child.PostLogoutRedirectUris.FromJson<List<string>>(),
                })
                .ToList();
            return items;
        }
    }
}
