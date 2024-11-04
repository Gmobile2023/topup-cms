using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.Products;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.Products;
using HLS.Topup.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Products)]
    public class ProductsController : TopupControllerBase
    {
        private readonly IProductsAppService _productsAppService;

        public ProductsController(IProductsAppService productsAppService)
        {
            _productsAppService = productsAppService;
        }

        public ActionResult Index()
        {
            var model = new ProductsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_Products_Create, AppPermissions.Pages_Products_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetProductForEditOutput getProductForEditOutput;

            if (id.HasValue)
            {
                getProductForEditOutput = await _productsAppService.GetProductForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getProductForEditOutput = new GetProductForEditOutput
                {
                    Product = new CreateOrEditProductDto()
                };
            }

            var viewModel = new CreateOrEditProductModalViewModel()
            {
                Product = getProductForEditOutput.Product,
                CategoryCategoryName = getProductForEditOutput.CategoryCategoryName,
                ProductCategoryList = await _productsAppService.GetAllCategoryForTableDropdown(),
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewProductModal(int id)
        {
            var getProductForViewDto = await _productsAppService.GetProductForView(id);

            var model = new ProductViewModel()
            {
                Product = getProductForViewDto.Product,
                CategoryCategoryName = getProductForViewDto.CategoryCategoryName
            };

            return PartialView("_ViewProductModal", model);
        }
    }
}