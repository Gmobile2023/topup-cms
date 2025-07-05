using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using HLS.Topup.AccountManager;
using HLS.Topup.Authorization;
using HLS.Topup.Authorization.Accounts;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Report;
using HLS.Topup.Reports;
using HLS.Topup.Web.Models.Report;
using Microsoft.AspNetCore.Mvc;

namespace HLS.Topup.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ReportController : TopupControllerBase
    {
        private readonly UserManager _userManager;
        private readonly IAccountManager _accountManager;
        private readonly IReportSystemAppService _reportAppService;
        private readonly ICommonLookupAppService _lookupAppService;
        private readonly IAgentService _agentSerrvie;
        public ReportController(UserManager userManager,
            IAccountManager accountManager,
            IReportSystemAppService reportAppService,
             IAgentService agentSerrvie,
            ICommonLookupAppService lookupAppService)
        {
            _userManager = userManager;
            _reportAppService = reportAppService;
            _lookupAppService = lookupAppService;
            _agentSerrvie = agentSerrvie;
            _accountManager = accountManager;
        }
        [AbpMvcAuthorize(AppPermissions.Pages_TransactionHistory)]
        public async Task<IActionResult> Detail()
        {           
            var user = await _accountManager.GetAccount(AbpSession.UserId ?? 0);
            var list = await _lookupAppService.GetListVendorTrans("");
            var module = new ReportViewModel()
            {
                User = user,
                Providers = (from x in list
                             select new ComboboxItemDto()
                             {
                                 Value = x.Code,
                                 DisplayText = x.Name,
                             }).ToList()
            };
            return View(module);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_Sale_Summary_Today)]
        public async Task<IActionResult> TotalDay()
        {
            return View();
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_Sale_Summary_DashBoard)]
        public async Task<IActionResult> DashRevenue()
        {
            var date = DateTime.Now;
            var data = new DashRevenueViewModel()
            {
                FromDate = new DateTime(date.Year, date.Month, 1),
                ToDate = date,
            };
            return View(data);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_CommissionAgentDetail)]
        public async Task<IActionResult> CommissionDetail()
        {
            var user = _userManager.GetAccountInfo();
            var module = new ReportRoseViewModel()
            {
                AccountCode = user.UserInfo.AccountCode,
            };
            return View(module);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_CommissionAgentTotal)]
        public async Task<IActionResult> CommissionTotal()
        {
            var user = _userManager.GetAccountInfo();
            var module = new ReportRoseViewModel()
            {
                AccountCode = user.UserInfo.AccountCode,
            };
            return View(module);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_CommissionAgentDash)]
        public async Task<IActionResult> DashAgentGeneral()
        {
            var date = DateTime.Now;
            var user = _userManager.GetAccountInfo();
            var data = new DashAgentGeneralViewModel()
            {
                FromDate = new DateTime(date.Year, date.Month, 1),
                ToDate = date,
                AgentCodeGeneral = user.UserInfo.AccountCode,
            };
            return View(data);
        }
    }
}
