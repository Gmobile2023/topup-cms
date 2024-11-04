using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.Discounts;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.DiscountManager;
using HLS.Topup.DiscountManager.Dtos;
using Abp.Application.Services.Dto;
using HLS.Topup.Products;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using HLS.Topup.DiscountManager.Importer;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Discounts)]
    public class DiscountsController : TopupControllerBase
    {
        private readonly IDiscountsAppService _discountsAppService;
        private readonly IDiscountListExcelDataReader _discountListExcelDataReader;

        public DiscountsController(IDiscountsAppService discountsAppService,
            IDiscountManger discountManger, IDiscountListExcelDataReader discountListExcelDataReader)
        {
            _discountsAppService = discountsAppService;
            _discountListExcelDataReader = discountListExcelDataReader;
        }

        public ActionResult Index()
        {
            var model = new DiscountsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_Discounts_Create, AppPermissions.Pages_Discounts_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id, bool isView = false)
        {
            GetDiscountForEditOutput getDiscountForEditOutput;

            if (id.HasValue)
            {
                getDiscountForEditOutput = await _discountsAppService.GetDiscountForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getDiscountForEditOutput = new GetDiscountForEditOutput
                {
                    Discount = new CreateOrEditDiscountDto()
                };
                getDiscountForEditOutput.Discount.FromDate = DateTime.Now;
                getDiscountForEditOutput.Discount.ToDate = DateTime.Now;
                getDiscountForEditOutput.Discount.DateApproved = DateTime.Now;
            }

            var viewModel = new CreateOrEditDiscountModalViewModel()
            {
                Discount = getDiscountForEditOutput.Discount,
                UserName = getDiscountForEditOutput.UserName,
                IsViewMode = isView,
                //DiscountUserList = await _discountsAppService.GetAllUserForTableDropdown(),
                ProductCategoryList = await _discountsAppService.GetAllCategoryForTableDropdown(),
                //ProductDiscounts = await _discountManger.GetDiscountDetails(id);
                DiscountServiceList = await _discountsAppService.GetAllServiceForTableDropdown()
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewDiscountModal(int id, bool isView = true)
        {
            var getDiscountForEditOutput = await _discountsAppService.GetDiscountForEdit(new EntityDto {Id = (int) id});
            var viewModel = new CreateOrEditDiscountModalViewModel()
            {
                Discount = getDiscountForEditOutput.Discount,
                UserName = getDiscountForEditOutput.UserName,
                IsViewMode = isView,
                ProductCategoryList = await _discountsAppService.GetAllCategoryForTableDropdown(),
                DiscountServiceList = await _discountsAppService.GetAllServiceForTableDropdown(),
                CreationTime = getDiscountForEditOutput.CreationTime,
                UserApproved = getDiscountForEditOutput.UserApproved,
                UserCreated = getDiscountForEditOutput.UserCreated
            };

            return PartialView("_ViewDiscountModal", viewModel);
        }
        
        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_Discounts_Create, AppPermissions.Pages_Discounts_Edit)]
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
                
                var dataList = _discountListExcelDataReader.GetDiscountFromExcel(fileBytes);
                
                if (dataList == null || !dataList.Any())
                {
                    throw new UserFriendlyException("Lỗi không load được dữ liệu file import");
                }

                return await _discountsAppService.GetDiscountImportList(dataList); 
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("Error"));
            }
        }
    }
}
