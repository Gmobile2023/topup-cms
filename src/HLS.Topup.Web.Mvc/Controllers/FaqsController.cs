using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Cms;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Web.Models.BillPayment;
using HLS.Topup.Web.Models.Faqs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServiceStack;

namespace HLS.Topup.Web.Controllers
{
    public class FaqsController : TopupControllerBase
    {
        private readonly ICmsAppService _cmsAppService;
        
        public FaqsController(ICmsAppService cmsAppService)
        {
            _cmsAppService = cmsAppService;
        }
        
        [AbpMvcAuthorize]
        public async Task<IActionResult> Index()
        {
            var data = await _cmsAppService.GetFaqsItems();
            var viewModel = new FaqsViewModel()
            {
                Items = data.ConvertTo<List<FaqsDto>>()
            };
            
            return View(viewModel);
        }
    }
}