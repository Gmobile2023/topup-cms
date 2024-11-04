using System.Linq;
using System;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using HLS.Topup.Authorization;
using HLS.Topup.Common;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Web.Areas.App.Models.CardStocks;
using HLS.Topup.Web.Areas.App.Models.Reports;
using HLS.Topup.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Authorization.Accounts;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    public class ReportCommissionController : TopupControllerBase
    {
        // public IActionResult Index()
        // {
        //     return View();
        // }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportCommissionDetail)]
        public async Task<IActionResult> CommissionDetail()
        {
            var module = new ReportCommissionViewModel()
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
            };

            return View(module);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportCommissionTotal)]
        public async Task<IActionResult> CommissionTotal()
        {
            var module = new ReportCommissionTotalViewModel()
            {
            };
            return View(module);
        }

    }
}
