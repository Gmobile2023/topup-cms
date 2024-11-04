using System;
using System.Net.Http;
using System.Threading.Tasks;
using Abp.Runtime.Caching;
using HLS.Topup.Configuration;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace HLS.Topup.Common
{
    public class TokenHepper : TopupDomainServiceBase
    {
        private readonly IConfigurationRoot _appConfiguration;
        private readonly ICacheManager _cacheManager;
        private readonly int _timeOut = 300;

        public TokenHepper(IWebHostEnvironment env, ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
            _appConfiguration = env.GetAppConfiguration();
        }

        // public async Task<string> GetAccessTokenViaCredentialsAsync1()
        // {
        //     return await _cacheManager.GetCache("TokenCode").GetAsync($"TokenApi", async () =>
        //     {
        //         try
        //         {
        //             var client = new HttpClient();
        //             var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
        //             {
        //                 Address = _appConfiguration["TopupService:TokenServer:Server"],
        //                 Policy =
        //                 {
        //                     ValidateIssuerName = false,
        //                     RequireHttps = true
        //                 }
        //             });
        //             if (disco.IsError)
        //             {
        //                 Logger.Error("disco error: ");
        //                 throw new Exception(disco.Error);
        //             }
        //
        //             //client.DefaultRequestHeaders.Add("Abp.TenantId", "1"); //Set TenantId
        //             var tokenResponse = await client.RequestClientCredentialsTokenAsync(
        //                 new ClientCredentialsTokenRequest
        //                 {
        //                     Address = disco.TokenEndpoint,
        //                     ClientId = _appConfiguration["TopupService:TokenServer:ClientId"],
        //                     ClientSecret = _appConfiguration["TopupService:TokenServer:SecrectKey"],
        //                     Scope = _appConfiguration["TopupService:TokenServer:ApiName"]
        //                 });
        //             return tokenResponse.IsError ? null : tokenResponse.AccessToken;
        //         }
        //         catch (Exception e)
        //         {
        //             Logger.Error("GetAccessTokenViaCredentialsAsync: " + e);
        //             return null;
        //         }
        //     });
        // }

        //Chỗ này hôm sau sửa lại Wrap vào 1 JsonServcieClient
        public TimeSpan GetTimeOut()
        {
            return TimeSpan.FromSeconds(_appConfiguration.GetValue<int>("TopupService:Timeout"));
        }
    }
}
