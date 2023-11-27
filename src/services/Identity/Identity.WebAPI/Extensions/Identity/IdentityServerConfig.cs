using IdentityServer4.Models;

namespace Identity.WebAPI.Extensions.Identity
{
    public class IdentityServerConfig
    {
        private const string ClientName = "client";
        public const string TasksApiScopeName = "TasksWebAPI";
        private const string ClientSecret = "client";
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope> { new(TasksApiScopeName) };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource> { new(TasksApiScopeName), };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = ClientName,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireClientSecret = false,
                    AllowedScopes = { TasksApiScopeName },
                    ClientSecrets = { new Secret(ClientSecret.Sha256()) },
                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true
                }
            };
    }
}
