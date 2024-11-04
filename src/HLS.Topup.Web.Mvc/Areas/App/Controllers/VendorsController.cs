using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.Vendors;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.Vendors;
using HLS.Topup.Vendors.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Vendors)]
    public class VendorsController : TopupControllerBase
    {
        private readonly IVendorsAppService _vendorsAppService;

        public VendorsController(IVendorsAppService vendorsAppService)
        {
            _vendorsAppService = vendorsAppService;
        }

        public ActionResult Index()
        {
            var model = new VendorsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_Vendors_Create, AppPermissions.Pages_Vendors_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetVendorForEditOutput getVendorForEditOutput;

				if (id.HasValue){
					getVendorForEditOutput = await _vendorsAppService.GetVendorForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getVendorForEditOutput = new GetVendorForEditOutput{
						Vendor = new CreateOrEditVendorDto()
					};
				}

				var viewModel = new CreateOrEditVendorModalViewModel()
				{
					Vendor = getVendorForEditOutput.Vendor,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewVendorModal(int id)
        {
			var getVendorForViewDto = await _vendorsAppService.GetVendorForView(id);

            var model = new VendorViewModel()
            {
                Vendor = getVendorForViewDto.Vendor
            };

            return PartialView("_ViewVendorModal", model);
        }


    }
}