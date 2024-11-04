using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.ServiceConfigurations;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.Configuration;
using HLS.Topup.Configuration.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using System.Collections.Generic;
using HLS.Topup.Authorization.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ServiceConfigurations)]
    public class ServiceConfigurationsController : TopupControllerBase
    {
        private readonly IServiceConfigurationsAppService _serviceConfigurationsAppService;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly TopupAppSession _session;

        public ServiceConfigurationsController(IServiceConfigurationsAppService serviceConfigurationsAppService, IWebHostEnvironment hostingEnvironment, TopupAppSession session)
        {
            _serviceConfigurationsAppService = serviceConfigurationsAppService;
            _appConfiguration = hostingEnvironment.GetAppConfiguration();
            _session = session;
        }

        public async Task<ActionResult> Index()
        {
            var model = new ServiceConfigurationsViewModel
            {
                FilterText = "",
                ServiceConfigurationServiceList =
                    await _serviceConfigurationsAppService.GetAllServiceForTableDropdown(),
                ServiceConfigurationProviderList =
                    await _serviceConfigurationsAppService.GetAllProviderForTableDropdown(),
                ServiceConfigurationCategoryList =
                    await _serviceConfigurationsAppService.GetAllCategoryForTableDropdown(),
                ServiceConfigurationProductList =
                    await _serviceConfigurationsAppService.GetAllProductForTableDropdown(),
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_ServiceConfigurations_Create,
            AppPermissions.Pages_ServiceConfigurations_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetServiceConfigurationForEditOutput getServiceConfigurationForEditOutput;

            if (id.HasValue)
            {
                getServiceConfigurationForEditOutput =
                    await _serviceConfigurationsAppService.GetServiceConfigurationForEdit(new EntityDto
                    { Id = (int)id });
            }
            else
            {
                getServiceConfigurationForEditOutput = new GetServiceConfigurationForEditOutput
                {
                    ServiceConfiguration = new CreateOrEditServiceConfigurationDto()
                };
            }

            var viewModel = new CreateOrEditServiceConfigurationModalViewModel()
            {
                ServiceConfiguration = getServiceConfigurationForEditOutput.ServiceConfiguration,
                ServiceServicesName = getServiceConfigurationForEditOutput.ServiceServicesName,
                ProviderName = getServiceConfigurationForEditOutput.ProviderName,
                CategoryCategoryName = getServiceConfigurationForEditOutput.CategoryCategoryName,
                ProductProductName = getServiceConfigurationForEditOutput.ProductProductName,
                UserName = getServiceConfigurationForEditOutput.UserName,
                ServiceConfigurationServiceList =
                    await _serviceConfigurationsAppService.GetAllServiceForTableDropdown(),
                ServiceConfigurationProviderList =
                    await _serviceConfigurationsAppService.GetAllProviderForTableDropdown(),
                ServiceConfigurationCategoryList =
                    await _serviceConfigurationsAppService.GetAllCategoryForTableDropdown(),
                StatusResponseConfigurationCategoryList = _serviceConfigurationsAppService.GetStatusResponseForTableDropdown()
            };

            var userName = _session.UserName.ToLower();
            var configValue = (_appConfiguration["App:SwichLoadUserRateValue"] ?? string.Empty).ToLower().Split(',', ';', '|').FirstOrDefault(c => c == userName);
            viewModel.IsDispalyRate = configValue != null;
            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewServiceConfigurationModal(int id)
        {
            var getServiceConfigurationForViewDto =
                await _serviceConfigurationsAppService.GetServiceConfigurationForView(id);

            var model = new ServiceConfigurationViewModel()
            {
                ServiceConfiguration = getServiceConfigurationForViewDto.ServiceConfiguration,
                ServiceServicesName = getServiceConfigurationForViewDto.ServiceServicesName,
                ProviderName = getServiceConfigurationForViewDto.ProviderName,
                CategoryCategoryName = getServiceConfigurationForViewDto.CategoryCategoryName,
                ProductProductName = getServiceConfigurationForViewDto.ProductProductName,
                UserName = getServiceConfigurationForViewDto.UserName
            };

            return PartialView("_ViewServiceConfigurationModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ServiceConfigurations_Create,
            AppPermissions.Pages_ServiceConfigurations_Edit)]
        public PartialViewResult ProductLookupTableModal(int? id, string displayName)
        {
            var viewModel = new ServiceConfigurationProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ServiceConfigurationProductLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ServiceConfigurations_Create,
            AppPermissions.Pages_ServiceConfigurations_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new ServiceConfigurationUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ServiceConfigurationUserLookupTableModal", viewModel);
        }
    }
}
