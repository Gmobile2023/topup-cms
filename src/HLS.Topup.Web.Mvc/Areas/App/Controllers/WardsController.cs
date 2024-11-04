using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.Wards;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.Address;
using HLS.Topup.Address.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Wards)]
    public class WardsController : TopupControllerBase
    {
        private readonly IWardsAppService _wardsAppService;

        public WardsController(IWardsAppService wardsAppService)
        {
            _wardsAppService = wardsAppService;
        }

        public ActionResult Index()
        {
            var model = new WardsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_Wards_Create, AppPermissions.Pages_Wards_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetWardForEditOutput getWardForEditOutput;

				if (id.HasValue){
					getWardForEditOutput = await _wardsAppService.GetWardForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getWardForEditOutput = new GetWardForEditOutput{
						Ward = new CreateOrEditWardDto()
					};
				}

				var viewModel = new CreateOrEditWardModalViewModel()
				{
					Ward = getWardForEditOutput.Ward,
					DistrictDistrictName = getWardForEditOutput.DistrictDistrictName,
					WardDistrictList = await _wardsAppService.GetAllDistrictForTableDropdown(),                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewWardModal(int id)
        {
			var getWardForViewDto = await _wardsAppService.GetWardForView(id);

            var model = new WardViewModel()
            {
                Ward = getWardForViewDto.Ward
                , DistrictDistrictName = getWardForViewDto.DistrictDistrictName 

            };

            return PartialView("_ViewWardModal", model);
        }


    }
}