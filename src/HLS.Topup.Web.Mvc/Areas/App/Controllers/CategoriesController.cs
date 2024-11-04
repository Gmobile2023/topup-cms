using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.Categories;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.Categories;
using HLS.Topup.Categories.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Categories)]
    public class CategoriesController : TopupControllerBase
    {
        private readonly ICategoriesAppService _categoriesAppService;

        public CategoriesController(ICategoriesAppService categoriesAppService)
        {
            _categoriesAppService = categoriesAppService;
        }

        public ActionResult Index()
        {
            var model = new CategoriesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Categories_Create, AppPermissions.Pages_Categories_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetCategoryForEditOutput getCategoryForEditOutput;

            if (id.HasValue)
            {
                getCategoryForEditOutput =
                    await _categoriesAppService.GetCategoryForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getCategoryForEditOutput = new GetCategoryForEditOutput
                {
                    Category = new CreateOrEditCategoryDto()
                };
            }

            var viewModel = new CreateOrEditCategoryModalViewModel()
            {
                Category = getCategoryForEditOutput.Category,
                CategoryCategoryName = getCategoryForEditOutput.CategoryCategoryName,
                ServiceServicesName = getCategoryForEditOutput.ServiceServicesName,
                CategoryCategoryList = await _categoriesAppService.GetAllCategoryForTableDropdown(),
                CategoryServiceList = await _categoriesAppService.GetAllServiceForTableDropdown(),
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCategoryModal(int id)
        {
            var getCategoryForViewDto = await _categoriesAppService.GetCategoryForView(id);

            var model = new CategoryViewModel()
            {
                Category = getCategoryForViewDto.Category,
                CategoryCategoryName = getCategoryForViewDto.CategoryCategoryName,
                ServiceServicesName = getCategoryForViewDto.ServiceServicesName
            };

            return PartialView("_ViewCategoryModal", model);
        }
    }
}