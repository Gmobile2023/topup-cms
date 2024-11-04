using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.Services;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.Services;
using HLS.Topup.Services.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Services)]
    public class ServicesController : TopupControllerBase
    {
        private readonly IServicesAppService _servicesAppService;

        public ServicesController(IServicesAppService servicesAppService)
        {
            _servicesAppService = servicesAppService;
        }

        public ActionResult Index()
        {
            var model = new ServicesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_Services_Create, AppPermissions.Pages_Services_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetServiceForEditOutput getServiceForEditOutput;

				if (id.HasValue){
					getServiceForEditOutput = await _servicesAppService.GetServiceForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getServiceForEditOutput = new GetServiceForEditOutput{
						Service = new CreateOrEditServiceDto()
					};
				}

				var viewModel = new CreateOrEditServiceModalViewModel()
				{
					Service = getServiceForEditOutput.Service,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewServiceModal(int id)
        {
			var getServiceForViewDto = await _servicesAppService.GetServiceForView(id);

            var model = new ServiceViewModel()
            {
                Service = getServiceForViewDto.Service
            };

            return PartialView("_ViewServiceModal", model);
        }


    }
}