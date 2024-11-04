using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.IO.Extensions;
using Abp.UI;
using HLS.Topup.Authorization;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.DiscountManager;
using HLS.Topup.Storage;
using HLS.Topup.Topup.Dtos;
using HLS.Topup.Web.Models.TopupRequest;
using HLS.Topup.Web.TagHelpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServiceStack;
using Aspose.Cells;
using HLS.Topup.Web.Models.Account;

namespace HLS.Topup.Web.Controllers
{
    [AbpMvcAuthorize]
    public class TopupController : TopupControllerBase
    {
        private readonly IViewRender _viewRender;
        private readonly ICommonManger _commonManger;
        private readonly IDiscountManger _discountManger;
        private readonly UserManager _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly ICommonLookupAppService _commonLookupAppService;

        public TopupController(IViewRender viewRender, ICommonManger commonManger, IDiscountManger discountManger,
            IWebHostEnvironment env, UserManager userManager, ICommonLookupAppService commonLookupAppService)
        {
            _viewRender = viewRender;
            _commonManger = commonManger;
            _discountManger = discountManger;
            _userManager = userManager;
            _commonLookupAppService = commonLookupAppService;
            _env = env;
        }

        // GET
        [AbpMvcAuthorize(AppPermissions.Pages_CreatePayment)]
        public async Task<IActionResult> Index()
        {
            var checkService = await _commonManger.CheckServiceActive(CommonConst.ServiceCodes.TOPUP);
            if (!checkService)
            {
                TempData["Type"] = CommonConst.MessageType.ServiceDisible;
                return RedirectToAction("Message", "Home");
            }

            var checkStaff = await _commonManger.CheckStaffTime(AbpSession.UserId ?? 0);
            if (!checkStaff)
            {
                TempData["Type"] = CommonConst.MessageType.StaffOutTime;
                return RedirectToAction("Message", "Home");
            }

            var cates = await _commonManger.GetCategories(CommonConst.ServiceCodes.TOPUP, null);
            var viewModel = new CreateOrEditTopupRequestModalViewModel()
            {
                TopupRequest = new CreateOrEditTopupRequestDto()
                {
                    ServiceCode = "TOPUP"
                },
                Categorys = cates.ConvertTo<List<CategoryDto>>()
            };
            return View(viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CreatePayment)]
        public async Task<IActionResult> PinCode()
        {
            var checkService = await _commonManger.CheckServiceActive(CommonConst.ServiceCodes.PIN_CODE);
            if (!checkService)
            {
                TempData["Type"] = CommonConst.MessageType.ServiceDisible;
                return RedirectToAction("Message", "Home");
            }

            var checkStaff = await _commonManger.CheckStaffTime(AbpSession.UserId ?? 0);
            if (!checkStaff)
            {
                TempData["Type"] = CommonConst.MessageType.StaffOutTime;
                return RedirectToAction("Message", "Home");
            }

            var pinCode = await _commonManger.GetCategories(CommonConst.ServiceCodes.PIN_CODE, null);
            var pinData = await _commonManger.GetCategories(CommonConst.ServiceCodes.PIN_DATA, null);
            var pinGame = await _commonManger.GetCategories(CommonConst.ServiceCodes.PIN_GAME, null);
            var viewModel = new CreateOrEditTopupRequestModalViewModel()
            {
                TopupRequest = new CreateOrEditTopupRequestDto(),
                PinCodeCategory = pinCode.ConvertTo<List<CategoryDto>>(),
                PinDataCategory = pinData.ConvertTo<List<CategoryDto>>(),
                PinGameCategory = pinGame.ConvertTo<List<CategoryDto>>()
            };

            return View(viewModel);
        }

        // [AbpMvcAuthorize(AppPermissions.Pages_CreatePayment)]
        // public async Task<IActionResult> PinData()
        // {
        //     var checkService = await _commonManger.CheckServiceActive(CommonConst.ServiceCodes.PIN_DATA);
        //     if (!checkService)
        //     {
        //         TempData["Type"] = CommonConst.MessageType.ServiceDisible;
        //         return RedirectToAction("Message", "Home");
        //     }
        //
        //     var checkStaff = await _commonManger.CheckStaffTime(AbpSession.UserId ?? 0);
        //     if (!checkStaff)
        //     {
        //         TempData["Type"] = CommonConst.MessageType.StaffOutTime;
        //         return RedirectToAction("Message", "Home");
        //     }
        //
        //     var cates = await _commonManger.GetCategories(CommonConst.ServiceCodes.PIN_DATA, null);
        //     var viewModel = new CreateOrEditTopupRequestModalViewModel()
        //     {
        //         TopupRequest = new CreateOrEditTopupRequestDto(),
        //         Categorys = cates.ConvertTo<List<CategoryDto>>()
        //     };
        //     return View(viewModel);
        // }

        // [AbpMvcAuthorize(AppPermissions.Pages_CreatePayment)]
        // public async Task<IActionResult> PinGame()
        // {
        //     var checkService = await _commonManger.CheckServiceActive(CommonConst.ServiceCodes.PIN_GAME);
        //     if (!checkService)
        //     {
        //         TempData["Type"] = CommonConst.MessageType.ServiceDisible;
        //         return RedirectToAction("Message", "Home");
        //     }
        //
        //     var checkStaff = await _commonManger.CheckStaffTime(AbpSession.UserId ?? 0);
        //     if (!checkStaff)
        //     {
        //         TempData["Type"] = CommonConst.MessageType.StaffOutTime;
        //         return RedirectToAction("Message", "Home");
        //     }
        //
        //     var cates = await _commonManger.GetCategories(CommonConst.ServiceCodes.PIN_GAME, null);
        //     var viewModel = new CreateOrEditTopupRequestModalViewModel()
        //     {
        //         TopupRequest = new CreateOrEditTopupRequestDto(),
        //         Categorys = cates.ConvertTo<List<CategoryDto>>()
        //     };
        //     return View(viewModel);
        // }

        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_CreatePayment)]
        public async Task<ResponseMessages> ImportTopupList()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                #region save file

                var basePath = Path.Combine(_env.ContentRootPath,
                    $"wwwroot{Path.DirectorySeparatorChar}assets{Path.DirectorySeparatorChar}Upload");
                if (!Directory.Exists(basePath)) Directory.CreateDirectory(basePath);
                var fileName = string.Format("{0}_{1:yyyyMMddHHmmssffff}_{2}",
                    "topup-list", DateTime.Now, file.FileName);
                var path = Path.Combine(basePath, fileName);

                await using (var stream = file.OpenReadStream())
                {
                    await using var originalFileStream = new MemoryStream(stream.GetAllBytes());
                    await using var zipEntryStream = System.IO.File.Create(path);
                    originalFileStream.CopyTo(zipEntryStream);
                }

                #endregion

                #region read file

                List<ImportTopupDto> data = new List<ImportTopupDto>();
                var msg = GetListInFile(path, ref data);
                if (data.Any())
                {
                    var cates = await _commonManger.GetCategories(CommonConst.ServiceCodes.TOPUP, null);
                    foreach (var item in data)
                    {
                        var cate = cates.FirstOrDefault(x => x.CategoryCode == item.CategoryCode);
                        if (cate != null)
                        {
                            item.CategoryName = cate.CategoryName;
                        }
                        else
                        {
                            item.CategoryCode = "";
                            item.CategoryName = "Nhà mạng không hợp lệ";
                        }
                    }
                }

                #endregion

                return new ResponseMessages
                {
                    ResponseCode = "01",
                    ResponseMessage = msg,
                    Payload = data
                };
            }
            catch (Exception ex)
            {
                return new ResponseMessages
                {
                    ResponseCode = "00",
                    ResponseMessage = L("Error")
                };
            }
        }


        [AbpMvcAuthorize(AppPermissions.Pages_CreatePayment)]
        public async Task<IActionResult> TopupData()
        {
            var checkService = await _commonManger.CheckServiceActive(CommonConst.ServiceCodes.TOPUP_DATA);
            if (!checkService)
            {
                TempData["Type"] = CommonConst.MessageType.ServiceDisible;
                return RedirectToAction("Message", "Home");
            }

            var checkStaff = await _commonManger.CheckStaffTime(AbpSession.UserId ?? 0);
            if (!checkStaff)
            {
                TempData["Type"] = CommonConst.MessageType.StaffOutTime;
                return RedirectToAction("Message", "Home");
            }

            var cates = await _commonManger.GetCategories(CommonConst.ServiceCodes.TOPUP_DATA, null);
            var viewModel = new CreateOrEditTopupRequestModalViewModel()
            {
                TopupRequest = new CreateOrEditTopupRequestDto()
                {
                    ServiceCode = "TOPUP_DATA"
                },
                Categorys = cates.ConvertTo<List<CategoryDto>>()
            };
            return View(viewModel);
        }

        protected string GetListInFile(string path, ref List<ImportTopupDto> data)
        {
            FileStream filestream = new FileStream(path, FileMode.Open);
            try
            {
                Worksheet worksheet = (new Workbook(filestream)).Worksheets[0];
                // add DataTable
                var dataTb = worksheet.Cells.ExportDataTable(0, 0,
                    worksheet.Cells.MaxDataRow + 1, worksheet.Cells.MaxDataColumn + 1);
                for (var i = 1; i < dataTb.Rows.Count; i++)
                {
                    var dataRow = dataTb.Rows[i];
                    data.Add(new ImportTopupDto()
                    {
                        CategoryCode = dataRow[1].ToString().Trim(),
                        CategoryName = dataRow[1].ToString().Trim(),
                        PhoneNumber = dataRow[2].ToString().Trim(),
                        CardPrice = Decimal.Parse(dataRow[3].ToString().Trim()),
                    });
                }

                filestream.Close();
                return "Thành công";
            }
            catch (Exception ex)
            {
                filestream.Close();
                return "Có lỗi trong quá trình upload file: " + ex.Message;
            }
        }

        // [AbpMvcAuthorize(AppPermissions.Pages_CreatePayment)]
        // public async Task<IActionResult> TopupList()
        // {
        //     var viewModel = new CreateOrEditTopupRequestModalViewModel()
        //     {
        //         TopupRequest = new Topup.Dtos.CreateOrEditTopupRequestDto(),
        //         Categorys = new List<CategoryDto>()
        //     };
        //     return View(viewModel);
        // }

        public async Task<JsonResult> GetTopupPrice(string catecode, string serviceCode)
        {
            try
            {
                var account = _userManager.GetAccountInfo().NetworkInfo;
                if (account == null || string.IsNullOrEmpty(account.AccountCode))
                    return Json(new
                    {
                        Content = string.Empty
                    });
                var model = new ProductServiceDto
                {
                    Products = await _commonLookupAppService.GetProductByCategory(catecode),
                    ServiceCode = serviceCode
                };
                var html = await _viewRender.RenderAsync("~/Views/Topup/_TopupPrice.cshtml",
                    model);
                return Json(new
                {
                    Content = html
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
