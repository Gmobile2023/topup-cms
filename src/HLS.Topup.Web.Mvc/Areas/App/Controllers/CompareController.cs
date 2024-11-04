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

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    public class CompareController : TopupControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
