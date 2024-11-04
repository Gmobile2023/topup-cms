using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.Cities;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.Address;
using HLS.Topup.Address.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Cities)]
    public class CitiesController : TopupControllerBase
    {
        private readonly ICitiesAppService _citiesAppService;

        public CitiesController(ICitiesAppService citiesAppService)
        {
            _citiesAppService = citiesAppService;
        }

        public ActionResult Index()
        {
            var model = new CitiesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_Cities_Create, AppPermissions.Pages_Cities_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetCityForEditOutput getCityForEditOutput;

				if (id.HasValue){
					getCityForEditOutput = await _citiesAppService.GetCityForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getCityForEditOutput = new GetCityForEditOutput{
						City = new CreateOrEditCityDto()
					};
				}

				var viewModel = new CreateOrEditCityModalViewModel()
				{
					City = getCityForEditOutput.City,
					CountryCountryName = getCityForEditOutput.CountryCountryName,
					CityCountryList = await _citiesAppService.GetAllCountryForTableDropdown(),                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewCityModal(int id)
        {
			var getCityForViewDto = await _citiesAppService.GetCityForView(id);

            var model = new CityViewModel()
            {
                City = getCityForViewDto.City
                , CountryCountryName = getCityForViewDto.CountryCountryName 

            };

            return PartialView("_ViewCityModal", model);
        }


    }
}