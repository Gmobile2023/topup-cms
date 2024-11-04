using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.SaleLimitDebts;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.Sale;
using HLS.Topup.Sale.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_SaleLimitDebts)]
    public class SaleLimitDebtsController : TopupControllerBase
    {
        private readonly ISaleLimitDebtsAppService _saleLimitDebtsAppService;

        public SaleLimitDebtsController(ISaleLimitDebtsAppService saleLimitDebtsAppService)
        {
            _saleLimitDebtsAppService = saleLimitDebtsAppService;
        }

        public ActionResult Index()
        {
            var model = new SaleLimitDebtsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_SaleLimitDebts_Create, AppPermissions.Pages_SaleLimitDebts_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetSaleLimitDebtForEditOutput getSaleLimitDebtForEditOutput;

            if (id.HasValue)
            {
                getSaleLimitDebtForEditOutput = await _saleLimitDebtsAppService.GetSaleLimitDebtForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getSaleLimitDebtForEditOutput = new GetSaleLimitDebtForEditOutput
                {
                    SaleLimitDebt = new CreateOrEditSaleLimitDebtDto()
                };
            }

            var viewModel = new CreateOrEditSaleLimitDebtModalViewModel()
            {
                SaleLimitDebt = getSaleLimitDebtForEditOutput.SaleLimitDebt,
                UserName = getSaleLimitDebtForEditOutput.UserName,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewSaleLimitDebtModal(int id)
        {
            var getSaleLimitDebtForViewDto = await _saleLimitDebtsAppService.GetSaleLimitDebtForView(id);

            var model = new SaleLimitDebtViewModel()
            {
                SaleLimitDebt = getSaleLimitDebtForViewDto.SaleLimitDebt
                ,
                UserName = getSaleLimitDebtForViewDto.UserName

            };

            return PartialView("_ViewSaleLimitDebtModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_SaleLimitDebts_Create, AppPermissions.Pages_SaleLimitDebts_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new SaleLimitDebtUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_SaleLimitDebtUserLookupTableModal", viewModel);
        }

    }
}