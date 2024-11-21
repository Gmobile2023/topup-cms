using System;
using System.Threading.Tasks;
using HLS.Topup.Configuration;
using HLS.Topup.Dtos.Cms;
using Microsoft.AspNetCore.Hosting;
using ServiceStack;

namespace HLS.Topup.Cms
{
    public class CmsManager : TopupDomainServiceBase, ICmsManager
    {
        private readonly string _cmsUrl;

        public CmsManager(IWebHostEnvironment env)
        {
            var appConfiguration = env.GetAppConfiguration();
            _cmsUrl = appConfiguration["CmsConfig:Url"];
        }

        public async Task<AcfAdvertiseAppDto> GetAdvertiseAcfByPage(int pageId)
        {
            var client = new JsonServiceClient(_cmsUrl)
            {
            };
            try
            {
                var result = await client.GetAsync<AcfAdvertiseAppDto>("acf/v3/pages/" + pageId);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"GetAdvertiseAcfByPage error: {pageId}. {ex}");
                return null;
            }
        }
        
        public async Task<AcfFaqsAppDto> GetFaqsAcfByPage(int pageId)
        {
            var client = new JsonServiceClient("https://cms.sandbox-topup.gmobile.vn/wp-json/")
            {
            };
            try
            {
                var result = await client.GetAsync<AcfFaqsAppDto>("acf/v3/pages/" + pageId);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error($"GetFaqsAcfByPage error: {pageId}. {ex}");
                return null;
            }
        }
    }
}
