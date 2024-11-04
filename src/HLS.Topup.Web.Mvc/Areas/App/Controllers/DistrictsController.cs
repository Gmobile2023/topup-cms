using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.Districts;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.Address;
using HLS.Topup.Address.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Districts)]
    public class DistrictsController : TopupControllerBase
    {
        private readonly IDistrictsAppService _districtsAppService;

        public DistrictsController(IDistrictsAppService districtsAppService)
        {
            _districtsAppService = districtsAppService;
        }

        public ActionResult Index()
        {
            var model = new DistrictsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_Districts_Create, AppPermissions.Pages_Districts_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetDistrictForEditOutput getDistrictForEditOutput;

				if (id.HasValue){
					getDistrictForEditOutput = await _districtsAppService.GetDistrictForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getDistrictForEditOutput = new GetDistrictForEditOutput{
						District = new CreateOrEditDistrictDto()
					};
				}

				var viewModel = new CreateOrEditDistrictModalViewModel()
				{
					District = getDistrictForEditOutput.District,
					CityCityName = getDistrictForEditOutput.CityCityName,
					DistrictCityList = await _districtsAppService.GetAllCityForTableDropdown(),                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewDistrictModal(int id)
        {
			var getDistrictForViewDto = await _districtsAppService.GetDistrictForView(id);

            var model = new DistrictViewModel()
            {
                District = getDistrictForViewDto.District
                , CityCityName = getDistrictForViewDto.CityCityName 

            };

            return PartialView("_ViewDistrictModal", model);
        }


    }
}