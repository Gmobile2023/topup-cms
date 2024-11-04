using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.SaleMans;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.Sale;
using HLS.Topup.Sale.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using HLS.Topup.Address;
using HLS.Topup.Address.Dtos;
using HLS.Topup.Dtos.Sale;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_SaleMans)]
    public class SaleMansController : TopupControllerBase
    {
        private readonly ISaleMansAppService _saleMansAppService;
        private readonly IDistrictsAppService _districtAppService;
        private readonly ISaleManManager _saleManManager;

        public SaleMansController(ISaleMansAppService saleMansAppService, IDistrictsAppService districtAppService, ISaleManManager saleManManager)
        {
            _saleMansAppService = saleMansAppService;
            _districtAppService = districtAppService;
            _saleManManager = saleManManager;
        }

        public ActionResult Index()
        {
            var model = new SaleMansViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_SaleMans_Create, AppPermissions.Pages_SaleMans_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            CreateOrUpdateSaleDto getSaleManForEditOutput;

            if (id.HasValue)
            {
                getSaleManForEditOutput = await _saleMansAppService.GetSaleManForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getSaleManForEditOutput = new CreateOrUpdateSaleDto
                {
                };
            }

            var viewModel = new CreateOrEditSaleManModalViewModel()
            {
                SaleMan = getSaleManForEditOutput,
                SaleManUserList = new List<SaleManUserLookupTableDto>()
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewSaleManModal(int id)
        {
            var getSaleManForViewDto = await _saleMansAppService.GetSaleManForView(id);           

            var model = new SaleManViewModel()
            {
                SaleMan = getSaleManForViewDto
            };
           
            model.SaleMan.SaleLeadName = getSaleManForViewDto.SaleLeadName;
            model.SaleMan.Description = getSaleManForViewDto.Description;

            return PartialView("_ViewSaleManModal", model);
        }
    }
}
