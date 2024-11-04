using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.AccountBlockBalances;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.BalanceManager;
using HLS.Topup.BalanceManager.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using HLS.Topup.RequestDtos;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_AccountBlockBalances)]
    public class AccountBlockBalancesController : TopupControllerBase
    {
        private readonly IAccountBlockBalancesAppService _accountBlockBalancesAppService;

        public AccountBlockBalancesController(IAccountBlockBalancesAppService accountBlockBalancesAppService)
        {
            _accountBlockBalancesAppService = accountBlockBalancesAppService;
        }

        public ActionResult Index()
        {
            var model = new AccountBlockBalancesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_AccountBlockBalances_Create,
            AppPermissions.Pages_AccountBlockBalances_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetAccountBlockBalanceForEditOutput getAccountBlockBalanceForEditOutput;

            if (id.HasValue)
            {
                getAccountBlockBalanceForEditOutput =
                    await _accountBlockBalancesAppService.GetAccountBlockBalanceForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getAccountBlockBalanceForEditOutput = new GetAccountBlockBalanceForEditOutput
                {
                    AccountBlockBalance = new CreateOrEditAccountBlockBalanceDto()
                };
            }

            var viewModel = new CreateOrEditAccountBlockBalanceModalViewModel()
            {
                AccountBlockBalance = getAccountBlockBalanceForEditOutput.AccountBlockBalance,
                UserName = getAccountBlockBalanceForEditOutput.UserName,
                AccountBlockBalanceUserList = await _accountBlockBalancesAppService.GetAllUserForTableDropdown(),
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewAccountBlockBalanceModal(int id)
        {
            var getAccountBlockBalanceForViewDto =
                await _accountBlockBalancesAppService.GetAccountBlockBalanceForView(id);

            var model = new AccountBlockBalanceViewModel()
            {
                AccountBlockBalance = getAccountBlockBalanceForViewDto.AccountBlockBalance,
                UserName = getAccountBlockBalanceForViewDto.UserName,
                AgentType = getAccountBlockBalanceForViewDto.AgentType,
                FullAgentName = getAccountBlockBalanceForViewDto.FullAgentName
            };

            return PartialView("_ViewAccountBlockBalanceModal", model);
        }
    }
}
