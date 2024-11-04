using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.LimitProducts;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.LimitationManager;
using HLS.Topup.LimitationManager.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using HLS.Topup.LimitationManager.Importer;
using HLS.Topup.Services;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_LimitProducts)]
    public class LimitProductsController : TopupControllerBase
    {
        private readonly ILimitProductsAppService _limitProductsAppService;
        private readonly IServicesAppService _lookup_serviceRepository;
        private readonly ILimitProductsListExcelDataReader _limitProductsListExcelDataReader;

        public LimitProductsController(ILimitProductsAppService limitProductsAppService,
            IServicesAppService lookup_serviceRepository,
            ILimitProductsListExcelDataReader limitProductsListExcelDataReader)
        {
            _limitProductsAppService = limitProductsAppService;
            _lookup_serviceRepository = lookup_serviceRepository;
            _limitProductsListExcelDataReader = limitProductsListExcelDataReader;
        }

        public async Task<ActionResult> Index()
        {
            var model = new LimitProductsViewModel
            {
                FilterText = "",
                LimitProductServiceList = await _limitProductsAppService.GetAllServiceForTableDropdown()
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_LimitProducts_Create, AppPermissions.Pages_LimitProducts_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetLimitProductForEditOutput getLimitProductForEditOutput;

            if (id.HasValue)
            {
                getLimitProductForEditOutput =
                    await _limitProductsAppService.GetLimitProductForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getLimitProductForEditOutput = new GetLimitProductForEditOutput
                {
                    LimitProduct = new CreateOrEditLimitProductDto()
                };
                getLimitProductForEditOutput.LimitProduct.FromDate = DateTime.Now;
                getLimitProductForEditOutput.LimitProduct.ToDate = DateTime.Now;
                getLimitProductForEditOutput.LimitProduct.DateApproved = DateTime.Now;
            }

            var viewModel = new CreateOrEditLimitProductModalViewModel()
            {
                LimitProduct = getLimitProductForEditOutput.LimitProduct,
                UserName = getLimitProductForEditOutput.UserName,
                LimitProductUserList = await _limitProductsAppService.GetAllUserForTableDropdown(),
                LimitProductServiceList = await _limitProductsAppService.GetAllServiceForTableDropdown()
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
        
        public async Task<PartialViewResult> ViewLimitProductModal(int id, bool isView = true)
        {
            var getLimitProductForViewDto = await _limitProductsAppService.GetLimitProductForEdit(new EntityDto {Id = (int) id});

            var model = new CreateOrEditLimitProductModalViewModel()
            {
                LimitProduct = getLimitProductForViewDto.LimitProduct,
                UserName = getLimitProductForViewDto.UserName,
                UserApproved = getLimitProductForViewDto.UserApproved,
                CreationTime = getLimitProductForViewDto.CreationTime,
                LimitProductServiceList = await _limitProductsAppService.GetAllServiceForTableDropdown(),
                AgentName = getLimitProductForViewDto.AgentName,
                IsViewMode = isView
            };

            return PartialView("_ViewLimitProductModal", model);
        }
        
        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_LimitProducts_Create, AppPermissions.Pages_LimitProducts_Edit)]
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
                
                var dataList = _limitProductsListExcelDataReader.GetLimitProductFromExcel(fileBytes);
                
                if (dataList == null || !dataList.Any())
                {
                    throw new UserFriendlyException("Lỗi không load được dữ liệu file import");
                }
                
                return await _limitProductsAppService.GetLimitProductImportList(dataList);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("Error"));
            }
        }
    }
}