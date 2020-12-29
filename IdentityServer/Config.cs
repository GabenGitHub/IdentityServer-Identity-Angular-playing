// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                // new ApiScope("scope1"),
                // new ApiScope("scope2"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // Angular client:
                new Client
                {
                    ClientName = "Angular-Client",
                    ClientId = "angular-client",

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:5003/signin-callback" },
                    RequirePkce = true,
                    // TODO Check this => AllowAccessTokensViaBrowser = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    },
                    AllowedCorsOrigins = { "https://localhost:5003" },
                    RequireClientSecret = true,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    PostLogoutRedirectUris = { "https://localhost:5003/signout-callback" },
                    RequireConsent = false,
                    AccessTokenLifetime = 3600 * 24,
                },
            };
    }
}