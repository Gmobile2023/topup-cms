using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.Providers;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.Providers;
using HLS.Topup.Providers.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using HLS.Topup.Dtos.Provider;
using HLS.Topup.Web.Areas.App.Models.StocksAirtimes;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using HLS.Topup.Configuration;
using HLS.Topup.Authorization.Users;
using System.Linq;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Providers)]
    public class ProvidersController : TopupControllerBase
    {
        private readonly IProvidersAppService _providersAppService;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly TopupAppSession _session;
        public ProvidersController(IProvidersAppService providersAppService, IWebHostEnvironment hostingEnvironment,
            TopupAppSession session)
        {
            _providersAppService = providersAppService;
            _appConfiguration = hostingEnvironment.GetAppConfiguration();
            _session = session;
        }

        public async Task<ActionResult> Index()
        {
            var model = new ProvidersViewModel
            {
                FilterText = "",
                ProviderList = await _providersAppService.GetAllProvider()
            };
            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_Providers_Create, AppPermissions.Pages_Providers_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetProviderForEditOutput getProviderForEditOutput;

            if (id.HasValue)
            {
                getProviderForEditOutput =
                    await _providersAppService.GetProviderForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getProviderForEditOutput = new GetProviderForEditOutput
                {
                    Provider = new CreateOrEditProviderDto(),
                    ProviderUpdate = new ProviderUpdateInfo()
                };
            }

            var viewModel = new CreateOrEditProviderModalViewModel()
            {
                Provider = getProviderForEditOutput.Provider,
                ProviderUpdate = getProviderForEditOutput.ProviderUpdate,
                ProviderList = await _providersAppService.GetAllProvider(),

            };

            var userName = _session.UserName.ToLower();
            var configValue = (_appConfiguration["App:SwichLoadUserRateValue"] ?? string.Empty).ToLower().Split(',', ';', '|').FirstOrDefault(c => c == userName);
            viewModel.IsDisplayRate = configValue != null;
            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewProviderModal(int id)
        {
            var getProviderForViewDto = await _providersAppService.GetProviderForView(id);

            var model = new ProviderViewModel()
            {
                Provider = getProviderForViewDto.Provider
            };

            return PartialView("_ViewProviderModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_StocksAirtimes_Deposit)]
        public async Task<PartialViewResult> DepositAirtimeModal(string keyCode, string providerCode)
        {
            var item = new CreateOrEditStocksAirtimeModalViewModel()
            {
                StocksAirtime = new StocksAirtimeDto()
                {
                    KeyCode = keyCode,
                    ProviderCode = providerCode
                }
            };
            return PartialView("_DepositAirtimeModal", item);
        }
    }
}