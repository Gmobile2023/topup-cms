using System;
using System.Collections.Generic;
using Abp.IdentityServer4;
using IdentityServer4.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Dtos.Authentication;
using HLS.Topup.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HLS.Topup.Web.IdentityServer
{
    public static class IdentityServerRegistrar
    {
        public static void Register(IServiceCollection services, IConfigurationRoot configuration,
            Action<IdentityServerOptions> setupOptions, List<IdentityServerStorageDto> clientIds)
        {
            services.AddIdentityServer(setupOptions)
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                .AddInMemoryClients(IdentityServerConfig.GetClients(clientIds))
                .AddAbpPersistedGrants<TopupDbContext>()
                .AddAbpIdentityServer<User>();
        }

        public static IIdentityServerBuilder AddIdentityServer(IServiceCollection services,
            IConfigurationRoot configuration,
            Action<IdentityServerOptions> setupOptions, string migrationsAssembly)
        {
            var builder = services.AddIdentityServer(setupOptions)
                // this adds the config data DB support (clients, resources, CORS)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseNpgsql(configuration["ConnectionStrings:IdentityServer"],
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data DB support (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseNpgsql(configuration["ConnectionStrings:IdentityServer"],
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // .AddAbpPersistedGrants<TopupDbContext>()
                .AddAbpIdentityServer<User>()
                .AddProfileService<ProfileService>();
            builder.AddCustomSigningCredential(configuration);
            builder.AddCustomValidationKey(configuration);
            //builder.AddExtensionGrantValidator<DelegationGrantValidator>();
            return builder;
        }

        public static void Register(IServiceCollection services, IConfigurationRoot configuration,
            List<IdentityServerStorageDto> clientIds)
        {
            Register(services, configuration, options => { }, clientIds);
        }
    }
}