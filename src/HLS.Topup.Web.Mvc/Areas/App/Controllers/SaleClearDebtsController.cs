using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.SaleClearDebts;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.Sale;
using HLS.Topup.Sale.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using HLS.Topup.Common;
using System.Linq;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_SaleClearDebts)]
    public class SaleClearDebtsController : TopupControllerBase
    {
        private readonly ISaleClearDebtsAppService _saleClearDebtsAppService;
        private readonly ICommonLookupAppService _commondAppService;

        public SaleClearDebtsController(ISaleClearDebtsAppService saleClearDebtsAppService,
            ICommonLookupAppService commondAppService)
        {
            _saleClearDebtsAppService = saleClearDebtsAppService;
            _commondAppService = commondAppService;
        }

        public async Task<ActionResult> Index()
        {
            var list = await _commondAppService.GetBanks();
            var banks = (from x in list
                        select new ComboboxItemDto
                        {

                            Value = x.Id.ToString(),
                            DisplayText = x.ShortName,
                        }).ToList();
            var model = new SaleClearDebtsViewModel
            {
                FilterText = "",
                Banks = banks,
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_SaleClearDebts_Create, AppPermissions.Pages_SaleClearDebts_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetSaleClearDebtForEditOutput getSaleClearDebtForEditOutput;

            if (id.HasValue)
            {
                getSaleClearDebtForEditOutput = await _saleClearDebtsAppService.GetSaleClearDebtForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getSaleClearDebtForEditOutput = new GetSaleClearDebtForEditOutput
                {
                    SaleClearDebt = new CreateOrEditSaleClearDebtDto()
                };
            }

            var viewModel = new CreateOrEditSaleClearDebtModalViewModel()
            {
                SaleClearDebt = getSaleClearDebtForEditOutput.SaleClearDebt,
                UserName = getSaleClearDebtForEditOutput.UserName,
                BankBankName = getSaleClearDebtForEditOutput.BankBankName,
                SaleClearDebtBankList = await _saleClearDebtsAppService.GetAllBankForTableDropdown(),
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewSaleClearDebtModal(int id)
        {
            var getSaleClearDebtForViewDto = await _saleClearDebtsAppService.GetSaleClearDebtForView(id);

            var model = new SaleClearDebtViewModel()
            {
                SaleClearDebt = getSaleClearDebtForViewDto.SaleClearDebt,
                UserName = getSaleClearDebtForViewDto.UserName,
                BankBankName = getSaleClearDebtForViewDto.BankBankName,
                StatusName = getSaleClearDebtForViewDto.StatusName,
            };

            return PartialView("_ViewSaleClearDebtModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_SaleClearDebts_Create, AppPermissions.Pages_SaleClearDebts_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new SaleClearDebtUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_SaleClearDebtUserLookupTableModal", viewModel);
        }

    }
}