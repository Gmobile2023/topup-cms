using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.IO.Extensions;
using Abp.UI;
using HLS.Topup.Authorization;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.DiscountManager;
using HLS.Topup.Topup.Dtos;
using HLS.Topup.Web.Models.TopupRequest;
using HLS.Topup.Web.TagHelpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServiceStack;
using Aspose.Cells;
using HLS.Topup.Transactions;
using HLS.Topup.FeeManager;
using HLS.Topup.Web.Models.Transaction;
using HLS.Topup.Dtos.Transactions;

namespace HLS.Topup.Web.Controllers
{
    [AbpMvcAuthorize]
    public class BatchTopupController : TopupControllerBase
    {
        private readonly IViewRender _viewRender;
        private readonly ICommonManger _commonManger;
        private readonly IDiscountManger _discountManger;
        private readonly IFeeManager _feeManager;
        private readonly UserManager _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly ITransactionsAppService _transactionSvc;
        private readonly TelcoHepper _telCoHepper;


        //Gunner. Chỗ này đưa nghiệp vụ vào service core xử lý + Chạy tiến trình backgroundJob
        public BatchTopupController(IViewRender viewRender,
            ICommonManger commonManger, IDiscountManger discountManger, IFeeManager feeManager,
            IWebHostEnvironment env, ITransactionsAppService transactionSvc, TelcoHepper telCoHepper,
            UserManager userManager)
        {
            _viewRender = viewRender;
            _commonManger = commonManger;
            _discountManger = discountManger;
            _feeManager = feeManager;
            _userManager = userManager;
            _env = env;
            _transactionSvc = transactionSvc;
            _appConfiguration = env.GetAppConfiguration();
            _telCoHepper = telCoHepper;
        }


        #region 0.Action
        [AbpMvcAuthorize(AppPermissions.Pages_CreateBatchLotPayment)]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CreateBatchLotPayment)]
        public async Task<IActionResult> AddCardModal()
        {
            //var list = await _commonLookupAppService.GetProvider();
            //var modal = new CompareViewModel()
            //{
            //    Providers = (from x in list
            //                 select new ComboboxItemDto()
            //                 {
            //                     Value = x.Code,
            //                     DisplayText = x.Name,
            //                 }).ToList(),
            //};

            return PartialView("_AddCardModal");
        }

        [AbpMvcAuthorize(AppPermissions.Pages_BatchLotHistory)]
        public async Task<IActionResult> TopupDetail(string batchCode)
        {
            var reponse = await _transactionSvc.GetBatchLotDetailList(new Transactions.Dtos.BatchDetailGetInput()
            {
                SkipCount = 0,
                MaxResultCount = int.MaxValue,
                BatchCode = batchCode,
                Status = -1,
                BatchStatus = -1
            });

            var list = reponse.Items.ConvertTo<List<BatchDetailDto>>();
            var model = new HistoryModalDetailViewModel()
            {
                BatchCode = batchCode,
                BatchName = "Nạp tiền điện thoại",
                BatchType = "TOPUP",
                Quantity = list.Count(),
                Price = list.Sum(c => c.PaymentAmount),
                QuantitySuccess = list.Count(c => c.Status == CommonConst.SaleRequestStatus.Success),
                PriceSuccess = list.Where(c => c.Status == CommonConst.SaleRequestStatus.Success).Sum(c => c.PaymentAmount),
            };
            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_BatchLotHistory)]
        public async Task<IActionResult> BillDetail(string batchCode)
        {
            var reponse = await _transactionSvc.GetBatchLotDetailList(new Transactions.Dtos.BatchDetailGetInput()
            {
                SkipCount = 0,
                MaxResultCount = int.MaxValue,
                BatchCode = batchCode,
                Status = -1,
                BatchStatus = -1
            });

            var list = reponse.Items.ConvertTo<List<BatchDetailDto>>();
            var model = new HistoryModalDetailViewModel()
            {
                BatchCode = batchCode,
                BatchName = "Thanh toán hóa đơn",
                BatchType = "PAYBILL",
                Quantity = list.Count(),
                Price = list.Sum(c => c.PaymentAmount),
                QuantitySuccess = list.Count(c => c.Status == CommonConst.SaleRequestStatus.Success),
                PriceSuccess = list.Where(c => c.Status == CommonConst.SaleRequestStatus.Success).Sum(c => c.PaymentAmount),
            };
            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_BatchLotHistory)]
        public async Task<IActionResult> PinDetail(string batchCode)
        {
            var reponse = await _transactionSvc.GetBatchLotDetailList(new Transactions.Dtos.BatchDetailGetInput()
            {
                SkipCount = 0,
                MaxResultCount = int.MaxValue,
                BatchCode = batchCode,
                Status = -1,
                BatchStatus = -1
            });

            var list = reponse.Items.ConvertTo<List<BatchDetailDto>>();
            var model = new HistoryModalDetailViewModel()
            {
                BatchCode = batchCode,
                BatchName = "Mua mã thẻ",
                BatchType = "PINCODE",
                Quantity = list.Sum(c => c.Quantity),
                Price = list.Sum(c => c.PaymentAmount),
                QuantitySuccess = list.Where(c => c.Status == CommonConst.SaleRequestStatus.Success).Sum(c => c.Quantity),
                PriceSuccess = list.Where(c => c.Status == CommonConst.SaleRequestStatus.Success).Sum(c => c.PaymentAmount),
            };
            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_BatchLotHistory)]
        public async Task<IActionResult> History()
        {
            return View();
        }

        public async Task<IActionResult> Result(BatchLotInfoModel model)
        {
            if (model.Code == "01")
            {
                var single = await _transactionSvc.GetBatchLotSingle(new Transactions.Dtos.BatchSingleInput()
                {
                    BatchCode = model.TransCode,
                });
                var dto = new TopupListModalViewModel()
                {
                    BatchCode = model.TransCode,
                    BatchName = single.BatchName,
                    Quantity = single.Quantity,
                    BatchType = single.BatchType,
                    Price = single.PaymentAmount,
                    Status = 1,
                };
                return View(dto);
            }
            else
            {
                var dto = new TopupListModalViewModel()
                {
                    Status = 0
                };
                return View(dto);
            }
        }

        #endregion

        #region 1.Query price

        [HttpPost]
        public async Task<ResponseMessages> QueryPriceTopupList(List<QueryItemDto> query)
        {
            try
            {
                #region Query

                var data = new List<SalePriceQueryDto>();

                var account = _userManager.GetAccountInfo().NetworkInfo;

                var productCodes = query.Select(c => c.ProductCode).Distinct();
                var isPayBill = query.First().IsPayBill;
                var lisDiscount = new List<SalePriceQueryDto>();
                var listFees = new List<Dtos.Fees.ProductFeeDto>();
                if (!string.IsNullOrEmpty(isPayBill) && isPayBill == "1")
                {
                    foreach (var productCode in productCodes)
                    {
                        var vs = query.Where(c => c.ProductCode == productCode).Select(c => c.Value).Distinct();

                        foreach (var v in vs)
                        {
                            var discount = await _discountManger.GeProductDiscountAccount(productCode, account.AccountCode, v);
                            if (discount != null)
                                lisDiscount.Add(new SalePriceQueryDto()
                                {
                                    Value = v,
                                    ProductCode = productCode,
                                    Discount = discount.DiscountAmount,
                                    Price = discount.PaymentAmount,
                                    Fee = 0,
                                });

                            var fee = await _feeManager.GetProductFee(productCode, account.AccountCode, v);
                            if (fee != null)
                                listFees.Add(fee);
                        }
                    }
                }
                else
                {
                    foreach (var productCode in productCodes)
                    {
                        var discount = await _discountManger.GeProductDiscountAccount(productCode, account.AccountCode);
                        if (discount != null)
                            lisDiscount.Add(new SalePriceQueryDto()
                            {
                                Value = discount.ProductValue,
                                ProductCode = productCode,
                                Discount = discount.DiscountAmount,
                                Price = discount.PaymentAmount,
                                Fee = 0,
                            });
                    }
                }


                foreach (var x in query)
                {
                    var product = lisDiscount.FirstOrDefault(c => c.ProductCode == x.ProductCode && c.Value == x.Value);
                    var item = new SalePriceQueryDto()
                    {
                        ProductCode = x.ProductCode,
                        ProductName = x.ProductName,
                        CategoryCode = x.CategoryCode,
                        CategoryName = x.CategoryName,
                        ServiceCode = x.ServiceCode,
                        ServiceName = x.ServiceName,
                        Value = x.Value,
                        Discount = (product?.Discount ?? 0) * (x.Quantity != 0 ? x.Quantity : 1),
                        Fee = 0,
                        Price = (product?.Price ?? x.Value) * (x.Quantity != 0 ? x.Quantity : 1),
                        ReceiverInfo = x.ReceiverInfo,
                        Quantity = x.Quantity,
                        Provider = "",
                    };
                    if (isPayBill == "1")
                    {
                        var fee = listFees.FirstOrDefault(c => c.ProductCode == x.ProductCode && c.Amount == x.Value);
                        if (fee != null)
                        {
                            item.Fee = fee.FeeValue;
                            item.Price += fee.FeeValue;
                        }
                    }
                    data.Add(item);
                }

                #endregion

                return new ResponseMessages
                {
                    ResponseCode = "01",
                    ResponseMessage = "",
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

        #endregion

        #region 2.Xử lý file

        public async Task<ResponseMessages> ImportTopupList(string batchType)
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
                var fileName = $"{"topup-list"}_{DateTime.Now:yyyyMMddHHmmssffff}_{file.FileName}";
                var path = Path.Combine(basePath, fileName);

                await using (var stream = file.OpenReadStream())
                {
                    await using var originalFileStream = new MemoryStream(stream.GetAllBytes());
                    await using var zipEntryStream = System.IO.File.Create(path);
                    await originalFileStream.CopyToAsync(zipEntryStream);
                }

                #endregion

                #region read file

                var data = new List<ImportBatchDto>();
                var listCates = new List<Categories.Category>();
                if (batchType == CommonConst.ServiceCodes.TOPUP)
                    listCates = await _commonManger.GetCategories(CommonConst.ServiceCodes.TOPUP, string.Empty);
                else if (batchType == "PINCODE")
                {
                    listCates = await _commonManger.GetCategories(CommonConst.ServiceCodes.PIN_CODE, string.Empty);
                    var listCates2 = await _commonManger.GetCategories(CommonConst.ServiceCodes.PIN_GAME, string.Empty);
                    var listCates3 = await _commonManger.GetCategories(CommonConst.ServiceCodes.PIN_DATA, string.Empty);
                    listCates.AddRange(listCates2);
                    listCates.AddRange(listCates3);
                }
                var msg = GetListInFile(path, batchType, ref data);
                if (data.Any())
                {
                    foreach (var item in data)
                    {
                        if (batchType == "PAYBILL")
                        {
                            var product = await _commonManger.GetProducts(item.ProductCode, string.Empty);
                            if (product.FirstOrDefault() != null)
                            {
                                var p = product.FirstOrDefault();
                                item.ProductName = p.ProductName;
                                item.CategoryCode = p.CategoryFk.CategoryCode;
                                item.CategoryName = p.CategoryFk.CategoryName;
                                item.ServiceCode = p.CategoryFk.ServiceFk.ServiceCode;
                                item.ServiceName = p.CategoryFk.ServiceFk.ServicesName;

                                if (item.Value <= 0)
                                {
                                    var info = QueryChargesData(item);
                                    item.Value = info.Value;
                                    item.Decription = info.Decription;
                                }
                                else if (item.CategoryCode != "MOBILE_BILL")
                                {
                                    var info = QueryChargesData(item);
                                    item.Value = info.Value;
                                    item.Decription = info.Decription;
                                }
                            }
                            else
                            {
                                item.CategoryCode = "";
                                item.CategoryName = "Sản phẩm không hợp lệ.";
                                msg = "File Import có nhà cung cấp không đúng hoặc đang bị khóa.";
                                data = new List<ImportBatchDto>();
                                return new ResponseMessages
                                {
                                    ResponseCode = "00",
                                    ResponseMessage = msg,
                                    Payload = data,
                                };
                            }
                        }
                        else
                        {
                            var cate = listCates.FirstOrDefault(x => x.CategoryCode == item.CategoryCode);
                            if (cate != null)
                            {
                                item.CategoryName = cate.CategoryName;
                                item.ServiceCode = cate.ServiceFk.ServiceCode;
                                item.ServiceName = cate.ServiceFk.ServicesName;
                            }
                            else
                            {
                                item.CategoryCode = "";
                                item.CategoryName = "Nhà mạng không hợp lệ";
                                msg = "Dữ liệu trong file không hợp lệ. Quý khách vui lòng kiểm tra lại.";
                                data = new List<ImportBatchDto>();
                                return new ResponseMessages
                                {
                                    ResponseCode = "00",
                                    ResponseMessage = msg,
                                    Payload = data,
                                };
                            }
                        }

                    }
                }
                else
                {
                    return new ResponseMessages
                    {
                        ResponseCode = "00",
                        ResponseMessage = msg,
                        Payload = data,
                    };
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

        private string GetListInFile(string path, string batchType, ref List<ImportBatchDto> data)
        {
            var filestream = new FileStream(path, FileMode.Open);
            try
            {

                var worksheet = (new Workbook(filestream)).Worksheets[0];
                var dataTb = worksheet.Cells.ExportDataTable(0, 0,
                    worksheet.Cells.MaxDataRow + 1, worksheet.Cells.MaxDataColumn + 1);
                for (var i = 1; i < dataTb.Rows.Count; i++)
                {
                    var dataRow = dataTb.Rows[i];
                    if (batchType == "TOPUP")
                    {

                        var cate = dataRow[2].ToString();
                        if (string.IsNullOrEmpty(cate))
                            cate = _telCoHepper.GetTelco(dataRow[1].ToString()?.Trim()) + "_TOPUP";
                        else
                        {
                            cate = _telCoHepper.GetVenderCode(cate) + "_TOPUP";
                            if (string.IsNullOrEmpty(cate))
                            {
                                data = new List<ImportBatchDto>();
                                return $"File Import có nhà mạng không đúng định dạng. Vui lòng kiểm tra lại nhà mạng.";
                            }
                        }

                        if (!_telCoHepper.CheckCardValue(dataRow[3].ToString()?.Trim()))
                        {
                            data = new List<ImportBatchDto>();
                            return $"File Import có mệnh giá không đúng. Vui lòng kiểm tra lại mệnh giá.";
                        }

                        if (dataRow[1].ToString().Trim().Length != 10 || !checkMobile(dataRow[1].ToString()?.Trim()))
                        {
                            data = new List<ImportBatchDto>();
                            return $"File Import có số điện thoại không đúng định dạng. Vui lòng kiểm tra lại số điện thoại.";
                        }

                        data.Add(new ImportBatchDto()
                        {
                            ProductCode = cate + "_" + (Decimal.Parse(dataRow[3].ToString()?.Trim() ?? string.Empty) / 1000).ToString(CultureInfo.InvariantCulture),
                            CategoryCode = cate,
                            CategoryName = cate,
                            ReceiverInfo = dataRow[1].ToString()?.Trim(),
                            Value = GetDecimalValue(dataRow[3].ToString()?.Trim()),
                            ProductName = "",
                            ServiceName = "",
                            ServiceCode = "TOPUP",
                            Quantity = 1,
                        });
                    }
                    else if (batchType == "PINCODE")
                    {
                        data.Add(new ImportBatchDto()
                        {
                            ProductCode = dataRow[1].ToString()?.Trim() + "_" + (Decimal.Parse(dataRow[2].ToString()?.Trim() ?? string.Empty) / 1000).ToString(CultureInfo.InvariantCulture),
                            CategoryCode = dataRow[1].ToString()?.Trim(),
                            CategoryName = dataRow[1].ToString()?.Trim(),
                            Value = GetDecimalValue(dataRow[2].ToString()?.Trim()),
                            Quantity = GetIntValue(dataRow[3].ToString()?.Trim()),
                        });
                    }
                    else if (batchType == "PAYBILL")
                    {
                        string description = "";
                        data.Add(new ImportBatchDto()
                        {
                            ProductCode = dataRow[1].ToString()?.Trim(),
                            ReceiverInfo = dataRow[2].ToString()?.Trim(),
                            CategoryCode = "",
                            CategoryName = "",
                            ProductName = "",
                            ServiceCode = "",
                            ServiceName = "",
                            Value = GetDecimalValue(dataRow[3].ToString()?.Trim()),
                            Quantity = 1,
                            Decription = description,
                        });
                    }
                }
                filestream.Close();

                var value = _telCoHepper.GetBatchLotValues(batchType);
                if (batchType == "TOPUP" || batchType == "PAYBILL")
                {
                    var v = value.Split(':');
                    if (data.Count() > Convert.ToInt32(v[1]))
                    {
                        data = new List<ImportBatchDto>();
                        return $"Quý khách chỉ được thực hiện số lượng giao dịch nhỏ hơn hoặc bằng {Convert.ToInt32(v[1])}.";
                    }

                    if (batchType == "PAYBILL")
                    {
                        var quantity = (from x in data
                            group x by x.ReceiverInfo into g
                            select new
                            {
                                key = g.Key,
                                count = g.Count()
                            }).Count(c => c.count >= 2);

                        if (quantity > 0)
                        {
                            data = new List<ImportBatchDto>();
                            return $"Danh sách của quý khách tồn tại mã hóa đơn lặp 2 lần.";
                        }
                    }
                }
                else if (batchType == "PINCODE")
                {
                    var v = value.Split(':')[1].Split('-');
                    if (data.Count() > Convert.ToInt32(v[0]))
                    {
                        data = new List<ImportBatchDto>();
                        return $"Quý khách chỉ được thực hiện số lượng giao dịch nhỏ hơn hoặc bằng {Convert.ToInt32(v[0])}.";
                    }

                    int count = data.Where(c => c.Quantity > Convert.ToInt32(v[1])).Count();
                    if (count > 0)
                    {
                        data = new List<ImportBatchDto>();
                        return $"Trên mỗi mệnh giá thẻ quý khách chỉ được phép mua số lượng nhỏ hơn hoặc bằng {Convert.ToInt32(v[1])}.";
                    }
                }

                if(data.Count==0)
                    return "Dữ liệu trong file không hợp lệ. Quý khách vui lòng kiểm tra lại.";
                return "Thành công";
            }
            catch (Exception ex)
            {
                filestream.Close();
                return "Có lỗi trong quá trình upload file: " + ex.Message;
            }
        }

        private decimal GetDecimalValue(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return 0;
                return Convert.ToDecimal(value);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private int GetIntValue(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return 0;
                return Convert.ToInt32(value);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private ImportBatchDto QueryChargesData(ImportBatchDto item)
        {
            try
            {
                var query = _transactionSvc.BillQueryRequest(new RequestDtos.BillQueryRequest()
                {
                    ReceiverInfo = item.ReceiverInfo,
                    ProductCode = item.ProductCode,
                    CategoryCode = item.CategoryCode,
                    ServiceCode = item.ServiceCode,
                }).Result;

                if (query != null)
                    item.Value = query.Amount;
                else item.Value = 0;
                if (item.Value == 0)
                    item.Decription = "Không tìm thấy cước.";
            }
            catch (Exception ex)
            {
                item.Decription = "Lỗi không tìm thấy hóa đơn.";
                item.Value = 0;
            }

            return item;
        }

        private bool checkMobile(string mobile)
        {
            string x = "0123456789";
            for (int i = 0; i < mobile.Length; i++)
            {
                if (!x.Contains(mobile[i].ToString()))
                    return false;
            }
            return true;
        }

        #endregion
    }
}
