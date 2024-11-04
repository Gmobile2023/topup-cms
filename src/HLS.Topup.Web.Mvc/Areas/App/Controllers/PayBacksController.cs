using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.IO.Extensions;
using Abp.UI;
using HLS.Topup.Authorization;
using HLS.Topup.PayBacks;
using HLS.Topup.PayBacks.Dtos;
using HLS.Topup.PayBacks.Importer;
using HLS.Topup.Web.Areas.App.Models.PayBacks;
using HLS.Topup.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_PayBacks)]
    public class PayBacksController : TopupControllerBase
    {
        private readonly IPayBacksAppService _payBacksAppService;
        private readonly IPayBacksListExcelDataReader _payBacksListExcelDataReader;

        public PayBacksController(IPayBacksAppService payBacksAppService,
            IPayBacksListExcelDataReader payBacksListExcelDataReader)
        {
            _payBacksAppService = payBacksAppService;
            _payBacksListExcelDataReader = payBacksListExcelDataReader;
        }

        public async Task<ActionResult> Index()
        {
            var model = new PayBacksViewModel
            {
                ProductCategory = await _payBacksAppService.GetAllCategoryForTableDropdown(),
                Provider = await _payBacksAppService.GetAllProviderForTableDropdown()
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_PayBacks_Create, AppPermissions.Pages_PayBacks_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetPayBackForEditOutput getPayBackForEditOutput;

            if (id.HasValue)
            {
                getPayBackForEditOutput = await _payBacksAppService.GetPayBacksForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getPayBackForEditOutput = new GetPayBackForEditOutput
                {
                    PayBacks = new CreateOrEditPayBacksDto()
                };
            }

            var viewModel = new CreateOrEditPayBacksModalViewModel()
            {
                PayBacks = getPayBackForEditOutput.PayBacks,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
        
        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_PayBacks_Create, AppPermissions.Pages_PayBacks_Edit)]
        public async Task<ResponseMessages> ReadFileImport()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                } 
                var dataList = _payBacksListExcelDataReader.GetPayBacksFromExcel(fileBytes);
                if (dataList == null || !dataList.Any())
                {
                    throw new UserFriendlyException("Lỗi không load được dữ liệu file import");
                }

                return await _payBacksAppService.GetPayBacksImportList(dataList); 
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("Error"));
            }
        }
        
        public async Task<PartialViewResult> ViewPayBacksModal(int id, bool isView = true)
        {
            var getPayBacksForViewDto = await _payBacksAppService.GetPayBacksForEdit(new EntityDto {Id = (int) id});

            var model = new CreateOrEditPayBacksModalViewModel()
            {
                PayBacks = getPayBacksForViewDto.PayBacks,
                UserName = getPayBacksForViewDto.UserName,
                UserApproved = getPayBacksForViewDto.UserApproved,
                CreationTime = getPayBacksForViewDto.CreationTime,
                IsViewMode = isView
            };

            return PartialView("_ViewPayBacksModal", model);
        }
    }
}