using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.Countries;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.Address;
using HLS.Topup.Address.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Countries)]
    public class CountriesController : TopupControllerBase
    {
        private readonly ICountriesAppService _countriesAppService;

        public CountriesController(ICountriesAppService countriesAppService)
        {
            _countriesAppService = countriesAppService;
        }

        public ActionResult Index()
        {
            var model = new CountriesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_Countries_Create, AppPermissions.Pages_Countries_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetCountryForEditOutput getCountryForEditOutput;

				if (id.HasValue){
					getCountryForEditOutput = await _countriesAppService.GetCountryForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getCountryForEditOutput = new GetCountryForEditOutput{
						Country = new CreateOrEditCountryDto()
					};
				}

				var viewModel = new CreateOrEditCountryModalViewModel()
				{
					Country = getCountryForEditOutput.Country,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewCountryModal(int id)
        {
			var getCountryForViewDto = await _countriesAppService.GetCountryForView(id);

            var model = new CountryViewModel()
            {
                Country = getCountryForViewDto.Country
            };

            return PartialView("_ViewCountryModal", model);
        }


    }
}