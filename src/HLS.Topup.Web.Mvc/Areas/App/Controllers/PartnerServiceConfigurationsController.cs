using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using HLS.Topup.Authorization;
using HLS.Topup.Configuration;
using HLS.Topup.Configuration.Dtos;
using HLS.Topup.Configuration.PartnerServiceConfigurationDtos;
using HLS.Topup.Web.Areas.App.Models.PartnerServiceConfigurations;
using HLS.Topup.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ServiceConfigurations)]
    public class PartnerServiceConfigurationsController : TopupControllerBase
    {
        private readonly IPartnerServiceConfigurationsAppService _serviceConfigurationsAppService;

        public PartnerServiceConfigurationsController(IPartnerServiceConfigurationsAppService serviceConfigurationsAppService)
        {
            _serviceConfigurationsAppService = serviceConfigurationsAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new PartnerServiceConfigurationsViewModel
            {
                FilterText = "",
                PartnerServiceConfigurationServiceList =
                    await _serviceConfigurationsAppService.GetAllServiceForTableDropdown(),
                PartnerServiceConfigurationProviderList =
                    await _serviceConfigurationsAppService.GetAllProviderForTableDropdown(),
                PartnerServiceConfigurationCategoryList =
                    await _serviceConfigurationsAppService.GetAllCategoryForTableDropdown(),
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ServiceConfigurations_Create,
            AppPermissions.Pages_ServiceConfigurations_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetPartnerServiceConfigurationForEditOutput getPartnerServiceConfigurationForEditOutput;

            if (id.HasValue)
            {
                getPartnerServiceConfigurationForEditOutput =
                    await _serviceConfigurationsAppService.GetPartnerServiceConfigurationForEdit(new EntityDto
                    { Id = (int)id });
            }
            else
            {
                getPartnerServiceConfigurationForEditOutput = new GetPartnerServiceConfigurationForEditOutput
                {
                    ServiceConfiguration = new CreateOrEditPartnerServiceConfigurationDto()
                };
            }

            var viewModel = new CreateOrEditPartnerServiceConfigurationModalViewModel()
            {
                ServiceConfiguration = getPartnerServiceConfigurationForEditOutput.ServiceConfiguration,
                ServiceServicesName = getPartnerServiceConfigurationForEditOutput.ServiceServicesName,
                ProviderName = getPartnerServiceConfigurationForEditOutput.ProviderName,
                CategoryCategoryName = getPartnerServiceConfigurationForEditOutput.CategoryCategoryName,
                UserName = getPartnerServiceConfigurationForEditOutput.UserName,
                PartnerServiceConfigurationServiceList =
                    await _serviceConfigurationsAppService.GetAllServiceForTableDropdown(),
                PartnerServiceConfigurationProviderList =
                    await _serviceConfigurationsAppService.GetAllProviderForTableDropdown(),
                PartnerServiceConfigurationCategoryList =
                    await _serviceConfigurationsAppService.GetAllCategoryForTableDropdown(),
                PartnerStatusResponseConfigurationCategoryList = _serviceConfigurationsAppService.GetStatusResponseForTableDropdown()
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewServiceConfigurationModal(int id)
        {
            var getServiceConfigurationForViewDto =
                await _serviceConfigurationsAppService.GetPartnerServiceConfigurationForView(id);

            var model = new PartnerServiceConfigurationViewModel()
            {
                ServiceConfiguration = getServiceConfigurationForViewDto.ServiceConfiguration,
                ServiceServicesName = getServiceConfigurationForViewDto.ServiceServicesName,
                ProviderName = getServiceConfigurationForViewDto.ProviderName,
                CategoryCategoryName = getServiceConfigurationForViewDto.CategoryCategoryName,
                UserName = getServiceConfigurationForViewDto.UserName
            };

            return PartialView("_ViewServiceConfigurationModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ServiceConfigurations_Create,
            AppPermissions.Pages_ServiceConfigurations_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PartnerServiceConfigurationUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ServiceConfigurationUserLookupTableModal", viewModel);
        }
    }
}