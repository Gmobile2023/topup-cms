using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.Fees;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.FeeManager;
using HLS.Topup.FeeManager.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using HLS.Topup.FeeManager.Importer;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Fees)]
    public class FeesController : TopupControllerBase
    {
        private readonly IFeesAppService _feesAppService;
        private readonly IFeeListExcelDataReader _feeListExcelDataReader;

        public FeesController(IFeesAppService feesAppService, IFeeListExcelDataReader feeListExcelDataReader)
        {
            _feesAppService = feesAppService;
            _feeListExcelDataReader = feeListExcelDataReader;
        }

        public ActionResult Index()
        {
            var model = new FeesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }
        
        [AbpMvcAuthorize(AppPermissions.Pages_Fees_Create, AppPermissions.Pages_Fees_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id, bool isView = false)
        {
            GetFeeForEditOutput getFeeForEditOutput;

            if (id.HasValue)
            {
                getFeeForEditOutput = await _feesAppService.GetFeeForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getFeeForEditOutput = new GetFeeForEditOutput
                {
                    Fee = new CreateOrEditFeeDto()
                };
                getFeeForEditOutput.Fee.FromDate = DateTime.Now;
                getFeeForEditOutput.Fee.ToDate = DateTime.Now;
                getFeeForEditOutput.Fee.DateApproved = DateTime.Now;
            }

            var viewModel = new CreateOrEditFeeModalViewModel()
            {
                Fee = getFeeForEditOutput.Fee,
                UserName = getFeeForEditOutput.UserName,
                FeeUserList = await _feesAppService.GetAllUserForTableDropdown(),
                IsViewMode = isView,
                FeeCategoryList = await _feesAppService.GetCategories()
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewFeeModal(int id, bool isView = true)
        {
            var getFeeForViewDto = await _feesAppService.GetFeeForEdit(new EntityDto {Id = (int) id});

            var model = new CreateOrEditFeeModalViewModel()
            {
                Fee = getFeeForViewDto.Fee,
                UserName = getFeeForViewDto.UserName,
                IsViewMode = isView,
                FeeUserList = await _feesAppService.GetAllUserForTableDropdown(),
                FeeCategoryList = await _feesAppService.GetCategories()
            };

            return PartialView("_ViewFeeModal", model);
        }
        
        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_Fees_Create, AppPermissions.Pages_Fees_Edit)]
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
                
                var dataList = _feeListExcelDataReader.GetFeeImportFromExcel(fileBytes);
                
                if (dataList == null || !dataList.Any())
                {
                    throw new UserFriendlyException("Lỗi không load được dữ liệu file import");
                }

                return await _feesAppService.GetFeeImportList(dataList); 
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("Error"));
            }
        }
    }
}