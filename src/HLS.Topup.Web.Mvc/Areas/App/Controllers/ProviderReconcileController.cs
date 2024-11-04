using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.IO.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Aspose.Cells;
using Hangfire;
using HLS.Topup.Authorization;
using HLS.Topup.Common;
using HLS.Topup.Compare;
using HLS.Topup.Providers;
using HLS.Topup.Providers.Importing;
using HLS.Topup.Storage;
using HLS.Topup.Web.Areas.App.Models.Providers;
using HLS.Topup.Web.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class ProviderReconcileController : TopupControllerBase
    {
        private readonly ILogger<ProviderReconcileController> _logger;
        private readonly ICommonLookupAppService _commonLookupAppService;
        private readonly ICompareAppService _compareSvc;

        public ProviderReconcileController(
            ILogger<ProviderReconcileController> logger,
            ICommonLookupAppService commonLookupAppService,
            ICompareAppService compareSvc)
        {
            _logger = logger;
            //_compareListExcelDataReader = compareListExcelDataReader;
            //_binaryObjectManager = binaryObjectManager;
            //_env = env;
            _compareSvc = compareSvc;
            _commonLookupAppService = commonLookupAppService;
        }

        // GET
        [AbpMvcAuthorize(AppPermissions.Pages_ProviderReconcile)]
        public async Task<IActionResult> Index()
        {
            var list = await _commonLookupAppService.GetProvider(isActive: false);
            var modal = new CompareViewModel()
            {
                Providers = (from x in list
                             select new ComboboxItemDto()
                             {
                                 Value = x.Code,
                                 DisplayText = x.Name,
                             }).ToList(),
            };
            return View(modal);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ProviderReconcile_Comapre)]
        public async Task<IActionResult> CreateOrEditModal()
        {
            var list = await _commonLookupAppService.GetProvider(isActive: false);
            var modal = new CompareViewModel()
            {
                Providers = (from x in list
                             select new ComboboxItemDto()
                             {
                                 Value = x.Code,
                                 DisplayText = x.Name,
                             }).ToList(),
            };

            return PartialView("_CreateOrEditModal", modal);
        }

        public async Task<IActionResult> ReponseModal(string providerCode, DateTime date, string keyCode)
        {
            var list = _compareSvc.GetCompareReponseList(new Providers.Dtos.GetCompareReponseInput()
            {
                ProviderCode = providerCode,
                TransDate = date,
            }).Result;

            var modal = new ReponseCompareViewModel()
            {
                TransDate = date,
                TransDateSearch = date.ToString("yyyyMMdd"),
                CompareDate = DateTime.Now,
                Provider = providerCode,
                KeyCode = keyCode,

            };
            modal.Items = list.Items.ToList();
            return PartialView("_ReponseModal", modal);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RefundsReconcile)]
        public async Task<IActionResult> Refund()
        {
            var list = await _commonLookupAppService.GetProvider(isActive: false);
            var modal = new RefundViewModel()
            {
                Providers = (from x in list
                             select new ComboboxItemDto()
                             {
                                 Value = x.Code,
                                 DisplayText = x.Name,
                             }).ToList(),
            };

            return View(modal);
        }

        public async Task<IActionResult> RefundDetailModal(string providerCode, DateTime date, string keyCode)
        {
            var refundSingle = await _compareSvc.GetCompareRefundSingle(new Providers.Dtos.GetCompareRefundSingleInput()
            {
                ProviderCode = providerCode,
                KeyCode = keyCode
            });
            var modal = new RefundCompareViewModel()
            {
                Provider = providerCode,
                TransDate = date,
                KeyCode = keyCode,
                TransDateSearch = date.ToString("yyyyMMdd"),
                CompareRefunDto = refundSingle
            };
            return PartialView("_RefundDetailModal", modal);
        }

        public async Task<ResponseMessages> ReadFileCompare(string providerCode, string transDate)
        {
            try
            {
                var reponse = await CompareFileMappingFull(providerCode, transDate);
                if (reponse.ResponseCode == "01")
                {
                    reponse.Payload = CompareViewData(reponse.Payload.ConvertTo<CompareProviderRequest>());
                    return reponse;
                }
                else return reponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode}|{transDate} ReadFileCompare error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return new ResponseMessages
                {
                    ResponseCode = "00",
                    ResponseMessage = L("Error")
                };
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ProviderReconcile_Comapre)]
        public async Task<ResponseMessages> ImportFileCompareFromJobExcel(string providerCode, string transDate)
        {
            try
            {
                var reponseCheck = await _compareSvc.CheckCompareProviderDate(new Providers.Dtos.ReportCheckCompareInput()
                {
                    ProviderCode = providerCode,
                    TransDate = transDate.Split('/')[2] + transDate.Split('/')[1] + transDate.Split('/')[0],
                });



                if (reponseCheck.ResponseCode != "01")
                    return reponseCheck;
                else if (Convert.ToInt32(reponseCheck.Payload) > 0)
                {
                    return new ResponseMessages
                    {
                        ResponseCode = "00",
                        ResponseMessage = $"Đã đối soát kênh {providerCode} của ngày giao dịch trên."
                    };
                }

                var reponse = await CompareFileMappingFull(providerCode, transDate);
                if (reponse.ResponseCode != "01")
                    return reponse;


                var reponseSync = _compareSvc.ImportCompareJob(reponse.Payload.ConvertTo<CompareProviderRequest>());
                //  BackgroundJob.Enqueue<ICompareAppService>((x) => x.ImportCompareJob(reponse.Payload.ConvertTo<CompareProviderRequest>(), AbpSession.ToUserIdentifier()));

                return new ResponseMessages
                {
                    ResponseCode = "01",
                    ResponseMessage = L("Bắt đầu đồng bộ dữ liệu đối soát, vui lòng chờ thông báo kết quả xử lý!")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode}|{transDate} ImportFileCompareFromJobExcel error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return new ResponseMessages
                {
                    ResponseCode = "00",
                    ResponseMessage = L("Error")
                };
            }
        }

        private async Task<ResponseMessages> CompareFileMappingFull(string providerCode, string transDate)
        {
            #region ValiDate

            var listSys = new List<CompareItem>();
            var listProvider = new List<CompareItem>();

            if (string.IsNullOrEmpty(providerCode))
            {
                return new ResponseMessages
                {
                    ResponseCode = "00",
                    ResponseMessage = "Quý khách chưa chọn nhà cung cấp"
                };
            }

            if (string.IsNullOrEmpty(transDate))
            {
                return new ResponseMessages
                {
                    ResponseCode = "00",
                    ResponseMessage = "Quý khách chưa nhập ngày giao dịch."
                };
            }

            var file_NCC = Request.Form.Files[0];
            if (file_NCC == null)
            {
                return new ResponseMessages
                {
                    ResponseCode = "00",
                    ResponseMessage = "Quý khách chưa chọn file Nhà cung cấp"
                };
            }
            else
            {

                var extension = Path.GetExtension(Request.Form.Files[0].FileName).ToLower();
                if (extension == ".csv" || extension == ".txt")
                {
                    if (providerCode.StartsWith("OCTA") || providerCode == "ZOTA" || providerCode == "NHATTRAN-TS"
                          || providerCode == "NHATTRAN" || providerCode == "IOMEDIA" || providerCode == "PAYOO"
                          || providerCode == "ESALE")
                    {

                        if (providerCode.StartsWith("OCTA"))
                            listProvider = ReadFileOCTA(providerCode);
                        else if (providerCode == "ZOTA")
                            listProvider = ReadFileZoTa(providerCode);
                        else if (providerCode == "NHATTRAN-TS" || providerCode == "NHATTRAN")
                            listProvider = ReadFileKpp(providerCode);
                        else if (providerCode == "IOMEDIA")
                            listProvider = ReadFileIOMediaCsv(providerCode);
                        else if (providerCode == "PAYOO")
                            listProvider = ReadFilePayooCsv(providerCode);
                        else if (providerCode == "ESALE")
                            listProvider = ReadFileESaleCsv(providerCode);

                        if (listProvider == null)
                        {
                            return new ResponseMessages
                            {
                                ResponseCode = "00",
                                ResponseMessage = $"Định dạng file NCC {providerCode} không hợp lệ.",
                            };
                        }
                    }
                }
                else if (extension == ".xls" || extension == ".xlsx")
                {
                    if (providerCode == "IOMEDIA" || providerCode == "MTC" || providerCode == "IMEDIA"
                     || providerCode == "IMEDIA2" || providerCode == "IMEDIA-TT"
                     || providerCode == "123CARD" || providerCode == "VIMO" || providerCode == "PAYTECH")
                    {
                        if (file_NCC.Length > 1048576 * 100 || file_NCC.Length > 1048576 * 100)
                        {
                            throw new UserFriendlyException(L("File_SizeLimit_Error"));
                        }

                        if (providerCode == "MTC")
                            listProvider = await ReadFileMTC(providerCode);
                        else if (providerCode == "IOMEDIA")
                            listProvider = await ReadFileIOMedia(providerCode);
                        else if (providerCode == "IMEDIA")
                            listProvider = await ReadFileIMedia_V2(providerCode);
                        else if (providerCode == "IMEDIA2" || providerCode == "IMEDIA-TT")
                            listProvider = await ReadFileIMedia2(providerCode);
                        else if (providerCode == "123CARD")
                            listProvider = await ReadFile123Card(providerCode);
                        else if (providerCode == "VIMO")
                            listProvider = await ReadFileVimo(providerCode);
                        else if (providerCode == "PAYTECH")
                            listProvider = await ReadFilePayTech(providerCode);

                    }
                }

                if (listProvider == null)
                {
                    return new ResponseMessages
                    {
                        ResponseCode = "00",
                        ResponseMessage = $"Định dạng file NCC {providerCode} không hợp lệ.",
                    };
                }
            }

            int length = Request.Form.Files.Count - 1;
            var file_NT = Request.Form.Files[length];
            if (file_NT == null)
            {
                return new ResponseMessages
                {
                    ResponseCode = "00",
                    ResponseMessage = "Quý khách chưa chọn file Nhất Trần"
                };
            }
            else
            {
                var extension = Path.GetExtension(file_NT.FileName).ToLower();
                if (extension != ".txt")
                {
                    return new ResponseMessages
                    {
                        ResponseCode = "00",
                        ResponseMessage = "Định dạng đuôi file của Nhất Trần không hợp lệ",
                    };
                }

                if (file_NT.Length > 1048576 * 100 || file_NT.Length > 1048576 * 100)
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                var split = file_NT.FileName.Split(".");
                if (split.Length < 4)
                {
                    return new ResponseMessages
                    {
                        ResponseCode = "00",
                        ResponseMessage = "Định dạng tên file NT không hợp lệ.",
                    };
                }

                if (split[1].Replace("NHATTRAN_", "") != providerCode)
                {
                    return new ResponseMessages
                    {
                        ResponseCode = "00",
                        ResponseMessage = "Mã nhà cung cấp không hợp lệ với tên file đối soát NT.",
                    };
                }

                if (GetDateTimeFile_NT(split[2]).Date != Convert.ToDateTime(transDate).Date)
                {
                    return new ResponseMessages
                    {
                        ResponseCode = "00",
                        ResponseMessage = "Ngày đối soát ở file NT và thời gian chọn ngày giao dịch đối soát không hợp lệ.",
                    };
                }

                listSys = ReadFile_NT(providerCode);
                if (listSys == null)
                {
                    return new ResponseMessages
                    {
                        ResponseCode = "00",
                        ResponseMessage = "Định dạng file NT không hợp lệ.",
                    };
                }

                if (listSys.FirstOrDefault() != null)
                {
                    if (listSys.First().TransDate.Date != GetDateTimeFile_NT(split[2]).Date)
                    {
                        return new ResponseMessages
                        {
                            ResponseCode = "00",
                            ResponseMessage = "Định dạng ngày trong file và ngày phát sinh giao dịch của NT không hợp lệ.",
                        };
                    }
                }

                if (listSys.FirstOrDefault() != null && listProvider.FirstOrDefault() != null)
                {
                    if (listSys.First().TransDate.Date != listProvider.First().TransDate.Date)
                    {
                        return new ResponseMessages
                        {
                            ResponseCode = "00",
                            ResponseMessage = "Ngày giao dịch trong file nhà cung cấp và ngày giao dịch của NT không hợp lệ.",
                        };
                    }
                }
                else if (listSys.FirstOrDefault() == null && listProvider.FirstOrDefault() == null)
                {
                    return new ResponseMessages
                    {
                        ResponseCode = "00",
                        ResponseMessage = "Không phát sinh giao dịch trong ngày đối soát.Vui lòng đối soát NCC khác.",
                    };
                }
            }

            #endregion

            var fileProviderName = FileNameNCC();
            var fileSysName = file_NT.FileName;
            var reponseCompare = CompareFileData(listSys, listProvider, providerCode, fileSysName, fileProviderName, Convert.ToDateTime(transDate), DateTime.Now);

            return new ResponseMessages()
            {
                ResponseCode = "01",
                ResponseMessage = "OKE",
                Payload = reponseCompare,
            };
        }

        private CompareProviderRequest CompareFileData(List<CompareItem> listSysNT, List<CompareItem> listProvider, string providerCode,
            string fileSys, string fileProvider, DateTime transDate, DateTime compareDate)
        {
            try
            {
                #region //Đối soát 
                var listSys = listSysNT.Where(c => c.IsRefund != true).ToList();
                var listSysRefund = listSysNT.Where(c => c.IsRefund == true).ToList();

                //Danh sach khop
                var same = (from p in listSys
                            join q in listProvider on p.TransCodePay equals q.TransCodePay
                            where p.SysValue == q.ProviderValue
                             && p.IsRefund == false
                            select new CompareItem()
                            {
                                Amount = p.Amount,
                                SysValue = p.SysValue,
                                ProviderCode = p.ProviderCode,
                                ProviderValue = q.ProviderValue,
                                ProductCode = p.ProductCode,
                                CompareDate = p.CompareDate,
                                AccountCode = p.AccountCode,
                                ReceivedAccount = p.ReceivedAccount,
                                TransCode = p.TransCode,
                                TransCodePay = p.TransCodePay,
                                TransDate = p.TransDate,
                                IsRefund = p.IsRefund,
                            }).ToList();

                //Danh sach lech
                var notSame = (from p in listSys
                               join q in listProvider on p.TransCodePay equals q.TransCodePay
                               where (p.SysValue != q.ProviderValue || p.IsRefund == true)
                               select new CompareItem
                               {
                                   Amount = p.Amount,
                                   SysValue = p.SysValue,
                                   ProviderCode = p.ProviderCode,
                                   ProviderValue = q.ProviderValue,
                                   ProductCode = p.ProductCode,
                                   CompareDate = p.CompareDate,
                                   AccountCode = p.AccountCode,
                                   ReceivedAccount = p.ReceivedAccount,
                                   TransCode = p.TransCode,
                                   TransCodePay = p.TransCodePay,
                                   TransDate = p.TransDate,
                                   IsRefund = p.IsRefund,
                               }).ToList();

                //Danh sach cac giao dich khong co transref, lech gio
                //Danh sách cac GD khong co tranref
                var listNotContaintranref = listSys.Where(p => string.IsNullOrEmpty(p.TransCodePay)).ToList();
                //Danh sách cac ban ghi ben doi tac chua co trong 2 danh sách lệch và khop
                var listPartner = (from p in listProvider
                                   join q in same on p.TransCodePay equals q.TransCodePay into g1
                                   from _g1 in g1.DefaultIfEmpty()
                                   join q1 in notSame on p.TransCodePay equals q1.TransCodePay into g2
                                   from _g2 in g2.DefaultIfEmpty()
                                   where _g1 == null && _g2 == null
                                   select p).ToList();

                //Danh sach không ghi nhan tranref nhung van thanh cong.
                //Duyet tung ban ghi o VNPOST so voi ben NCC

                var listTimeOut = (from p in listNotContaintranref
                                   join q in listPartner
                                       on new { p.ReceivedAccount, p.Amount } equals new { q.ReceivedAccount, q.Amount }
                                   where Math.Abs((p.TransDate - q.TransDate).TotalMinutes) < 10
                                   select new
                                   {
                                       p.TransCode,
                                       q.TransCodePay
                                   }).ToList();

                //Loai bo cac GD trung nhau
                var listTrancode = listTimeOut.GroupBy(p => p.TransCodePay).Select(p => p.Key).ToString();

                //Them vao GD khop
                same.AddRange((from p in listSys
                               join q in listTrancode on
                               p.TransCode equals q.ToString()
                               select p).ToList());

                //NCC co NT ko co
                var NCC = (from p in listProvider
                           join q in listSys on p.TransCodePay equals q.TransCodePay into g
                           from j in g.DefaultIfEmpty()
                           join q1 in listTimeOut on p.TransCodePay equals q1.TransCodePay into g1
                           from j1 in g1.DefaultIfEmpty()
                           where j == null && j1 == null
                           select p).ToList();

                //NT co NCC ko co
                var sysCo = (from p in listSys
                             join q in listProvider on p.TransCodePay equals q.TransCodePay into g
                             from j in g.DefaultIfEmpty()
                             join q1 in listTimeOut on p.TransCodePay equals q1.TransCodePay into g1
                             from j1 in g1.DefaultIfEmpty()
                             where j == null && j1 == null
                             select p).ToList();

                var listSame = same.Select(item => new CompareItem
                {
                    AccountCode = item.AccountCode,
                    TransCode = item.TransCode,
                    TransDate = item.TransDate,
                    TransCodePay = item.TransCodePay,
                    Amount = item.Amount,
                    SysValue = item.SysValue,
                    ProductCode = item.ProductCode,
                    ReceivedAccount = item.ReceivedAccount,
                    CompareDate = DateTime.Now,
                    ProductName = item.ProductName,
                    ProviderCode = item.ProviderCode,
                    ProviderValue = item.ProviderValue,
                    Status = 1,
                    Result = 1,
                    IsRefund = item.IsRefund,
                }).ToList();

                var listNotSame = notSame.Select(item => new CompareItem
                {
                    AccountCode = item.AccountCode,
                    TransCode = item.TransCode,
                    TransDate = item.TransDate,
                    SysValue = item.SysValue,
                    Status = 1,
                    Result = 3,
                    ProviderValue = item.ProviderValue,
                    ReceivedAccount = item.ReceivedAccount,
                    CompareDate = DateTime.Now,
                    ProductCode = item.ProductCode,
                    Amount = item.Amount,
                    ProviderCode = item.ProviderCode,
                    TransCodePay = item.TransCodePay,
                    ProductName = item.ProductName,
                    IsRefund = item.IsRefund,

                }).ToList();

                var listNCC = NCC.Select(item => new CompareItem
                {
                    AccountCode = item.AccountCode,
                    TransDate = item.TransDate,
                    Amount = item.Amount,
                    SysValue = item.SysValue,
                    Status = 1,
                    Result = 2,
                    ProviderValue = item.ProviderValue,
                    CompareDate = DateTime.Now,
                    ReceivedAccount = item.ReceivedAccount,
                    ProductCode = item.ProductCode,
                    ProviderCode = item.ProviderCode,
                    TransCode = item.TransCode,
                    TransCodePay = item.TransCodePay,
                    ProductName = item.ProductName,
                    IsRefund = false,
                }).ToList();
                var listNT = sysCo.Select(item => new CompareItem
                {
                    AccountCode = item.AccountCode,
                    TransDate = item.TransDate,
                    Amount = item.Amount,
                    SysValue = item.SysValue,
                    Status = 1,
                    Result = 0,
                    ProviderValue = item.ProviderValue,
                    CompareDate = DateTime.Now,
                    ReceivedAccount = item.ReceivedAccount,
                    ProductCode = item.ProductCode,
                    ProviderCode = item.ProviderCode,
                    TransCode = item.TransCode,
                    TransCodePay = item.TransCodePay,
                    ProductName = item.ProductName,
                    IsRefund = item.IsRefund,
                }).ToList();

                listSame.AddRange(listNCC);
                listSame.AddRange(listNT);
                listSame.AddRange(listNotSame);

                //Ghi lich su doi soat
                var listCompare = new CompareProviderRequest
                {
                    CompareDate = compareDate,
                    TransDate = transDate,
                    Isenabled = true,
                    ProviderFileName = fileProvider,
                    SysFileName = fileSys,
                    ProviderCode = providerCode,
                    SameQuantity = same.Count(),
                    SameAmount = same.Sum(c => c.SysValue),
                    ProviderQuantity = listProvider.Count(),
                    ProviderAmount = listProvider.Sum(c => c.ProviderValue),
                    SysAmount = listSys.Sum(c => c.SysValue),
                    SysQuantity = listSys.Count(),
                    NotSameQuantity = notSame.Count,
                    NotSameSysAmount = listNotSame.Sum(p => Math.Abs(p.SysValue)),
                    NotSameProviderAmount = listNotSame.Sum(p => Math.Abs(p.ProviderValue)),
                    ProviderOnlyQuantity = NCC.Count(),
                    ProviderOnlyAmount = NCC.Sum(c => c.ProviderValue),
                    SysOnlyQuantity = sysCo.Count(),
                    SysOnlyAmount = sysCo.Sum(c => c.SysValue),
                    Items = listSame,
                    RefundAutoQuantity = listSysRefund.Count(),
                    RefundAutoAmount = listSysRefund.Sum(c => c.SysValue)
                };

                return listCompare;
                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode} CompareFileData error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return new CompareProviderRequest();
            }
        }

        private List<CompareFileDto> CompareViewData(CompareProviderRequest request)
        {
            var li = new List<CompareFileDto>();
            li.Add(new CompareFileDto()
            {
                CompareType = "BF Nhất Trần",
                Quantity = request.SysQuantity,
                Amount = request.SysAmount,
                QuantityRefund = request.RefundAutoQuantity,
                AmountRefund = request.RefundAutoAmount,
                //QuantityRefund = request.Items.Where(c => c.Status == 1 && c.IsRefund == true).Count(),
                //AmountRefund = request.Items.Where(c => c.Status == 1 && c.IsRefund == true).Sum(c => c.SysValue),
            });

            li.Add(new CompareFileDto()
            {
                CompareType = "BF NCC",
                Quantity = request.ProviderQuantity,
                Amount = request.ProviderAmount,
                QuantityRefund = 0,
                AmountRefund = 0,
            });

            return li;
        }

        #region A.Xử lý file để Compare

        #region 1.NT .txt

        private List<CompareItem> ReadFile_NT(string providerCode)
        {
            try
            {
                int length = Request.Form.Files.Count - 1;
                var file = Request.Form.Files[length];
                var compareDate = DateTime.Now;
                List<CompareItem> lst = new List<CompareItem>();

                string fileExtension = Path.GetExtension(file.FileName);
                using (var sreader = new StreamReader(file.OpenReadStream()))
                {
                    string[] headers = sreader.ReadLine().Split(',');
                    while (!sreader.EndOfStream)
                    {
                        var line = sreader.ReadLine();
                        var item = line.Split(",");
                        lst.Add(new CompareItem()
                        {
                            TransDate = GetDateTime_NT(item[1]),
                            TransCode = item[2],
                            AccountCode = item[4],
                            ProviderCode = providerCode,
                            ProductCode = item[8],
                            SysValue = Convert.ToDecimal(item[9]) * (item.Length >= 19 && !string.IsNullOrEmpty(item[18]) ? Convert.ToInt32(item[18]) : 1),
                            Amount = Convert.ToDecimal(item[12]),
                            ReceivedAccount = item[13],
                            TransCodePay = item[15],
                            CompareDate = compareDate,
                            IsRefund = item.Length >= 18 && item[17] == "1" ? true : false,
                        });
                    }
                }

                return lst;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode} ReadFile_NT error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return null;
            }

        }

        #endregion

        #region 2.ZOTA .csv

        private List<CompareItem> ReadFileZoTa(string providerCode)
        {
            try
            {

                var compareDate = DateTime.Now;
                List<CompareItem> lst = new List<CompareItem>();
                //foreach (var fileItem in Request.Form.Files)
                for (int i = 0; i < Request.Form.Files.Count - 1; i++)
                {
                    var file = Request.Form.Files[i];
                    string fileExtension = Path.GetExtension(file.FileName);
                    using (var sreader = new StreamReader(file.OpenReadStream()))
                    {
                        string[] headers = sreader.ReadLine().Split(',');
                        while (!sreader.EndOfStream)
                        {
                            var line = sreader.ReadLine();
                            if (!string.IsNullOrEmpty(line))
                            {
                                var item = line.Split(",");
                                if ((item[2].ToUpper() != "FUND_IN" || item[2].ToUpper() != "TXN_REVERSAL") && item[7].ToUpper() == "CLOSED")
                                {

                                    lst.Add(new CompareItem()
                                    {
                                        TransDate = DateTime.Parse(item[1]),
                                        Amount = 0,
                                        ProviderValue = Convert.ToDecimal(item[3]),
                                        Quantity = Convert.ToInt32(!string.IsNullOrEmpty(item[9]) ? item[9] : "1"),
                                        ReceivedAccount = string.Empty,
                                        TransCodePay = item[6].Replace("'", ""),
                                        CompareDate = compareDate,
                                        ProviderCode = providerCode,
                                    });
                                }
                            }
                        }
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode} ReadFileZoTa error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return null;
            }

        }
        #endregion

        #region 3.OCTA .csv

        private List<CompareItem> ReadFileOCTA(string providerCode)
        {
            try
            {

                var compareDate = DateTime.Now;
                List<CompareItem> lst = new List<CompareItem>();
                for (int i = 0; i < Request.Form.Files.Count - 1; i++)
                {
                    var file = Request.Form.Files[i];
                    string fileExtension = Path.GetExtension(file.FileName);
                    using (var sreader = new StreamReader(file.OpenReadStream()))
                    {
                        string[] headers = sreader.ReadLine().Split(',');
                        while (!sreader.EndOfStream)
                        {
                            var line = sreader.ReadLine();
                            if (!string.IsNullOrEmpty(line))
                            {
                                var item = line.Split(",");
                                if (item[7] == "Thành công")
                                {
                                    lst.Add(new CompareItem()
                                    {
                                        TransDate = GetDateTime_Octa(item[0]),
                                        TransCodePay = item[2],
                                        ReceivedAccount = item[5],
                                        Quantity = Convert.ToInt32(!string.IsNullOrEmpty(item[10]) ? item[10] : "1"),
                                        ProviderValue = Convert.ToDecimal(item[11]),
                                        Amount = Convert.ToDecimal(item[12]),
                                        CompareDate = compareDate,
                                        ProviderCode = providerCode,
                                    });
                                }
                            }
                        }
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode} ReadFileOCTA error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return null;
            }

        }
        #endregion

        #region 4.MTC .xls

        private async Task<List<CompareItem>> ReadFileMTC(string providerCode)
        {
            try
            {
                List<CompareItem> lst = new List<CompareItem>();
                for (int i = 0; i < Request.Form.Files.Count - 1; i++)
                {
                    var file = Request.Form.Files[i];
                    var desFileCsv = await GetFileCSV(file, providerCode, retry: 0);
                    var compareDate = DateTime.Now;
                    using (var sreader = System.IO.File.OpenRead(desFileCsv))
                    {
                        var lines = sreader.ReadLines();
                        foreach (var item in lines)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                var line = item.Replace('"', ' ').Split(',');
                                if (line.Length >= 8 && line[5] == "SUCCESS")
                                {
                                    lst.Add(new CompareItem()
                                    {
                                        TransDate = DateTime.Parse(line[8]),
                                        TransCodePay = line[7],
                                        ReceivedAccount = line[1],
                                        ProviderValue = Convert.ToDecimal(line[4]),
                                        Amount = Convert.ToDecimal(line[4]),
                                        CompareDate = compareDate,
                                        ProviderCode = providerCode,
                                    });
                                }
                                else if (line.Length >= 8 && line[6] == "SUCCESS")
                                {
                                    lst.Add(new CompareItem()
                                    {
                                        TransDate = DateTime.Parse(line[9]),
                                        TransCodePay = line[8],
                                        ReceivedAccount = line[1],
                                        ProviderValue = Convert.ToDecimal(line[4] + line[5]),
                                        Amount = Convert.ToDecimal(line[4] + line[5]),
                                        CompareDate = compareDate,
                                        ProviderCode = providerCode,
                                    });
                                }
                            }

                        }
                    }
                    System.IO.File.Delete(desFileCsv);
                }
                return lst;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode} ReadFileMTC error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return null;
            }

        }
        #endregion

        #region 5.IOMedia .xls

        private async Task<List<CompareItem>> ReadFileIOMedia(string providerCode)
        {
            try
            {
                List<CompareItem> lst = new List<CompareItem>();
                //foreach (var fileItem in Request.Form.Files)
                for (int i = 0; i < Request.Form.Files.Count - 1; i++)
                {
                    var file = Request.Form.Files[i];
                    var desFileCsv = await GetFileCSV(file, providerCode, retry: 0);
                    var compareDate = DateTime.Now;
                    int index = 0;
                    using (var sreader = System.IO.File.OpenRead(desFileCsv))
                    {
                        var lines = sreader.ReadLines();
                        foreach (var item in lines)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                var line = item.Split(',');
                                if (line.Length >= 11 && index > 0)
                                {
                                    lst.Add(new CompareItem()
                                    {
                                        TransDate = GetDateTime_Imedia(line[9]),
                                        TransCodePay = line[10],
                                        ReceivedAccount = line[4],
                                        ProviderValue = Convert.ToDecimal(line[3]),
                                        Amount = Convert.ToDecimal(line[8]),
                                        CompareDate = compareDate,
                                        ProviderCode = providerCode,
                                    });
                                }
                                else if (line.Length >= 10 && index > 0)
                                {
                                    lst.Add(new CompareItem()
                                    {
                                        TransDate = GetDateTime_Imedia(line[8]),
                                        TransCodePay = line[9],
                                        ReceivedAccount = line[4],
                                        ProviderValue = Convert.ToDecimal(line[3]),
                                        Amount = Convert.ToDecimal(line[7]),
                                        CompareDate = compareDate,
                                        ProviderCode = providerCode,
                                    });
                                }
                                index += 1;
                            }

                        }
                    }
                    System.IO.File.Delete(desFileCsv);
                }
                return lst;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode} ReadFileIOMedia error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return null;
            }

        }
        private List<CompareItem> ReadFileIOMediaCsv(string providerCode)
        {
            try
            {

                var compareDate = DateTime.Now;
                List<CompareItem> lst = new List<CompareItem>();
                List<CompareItem> lstCard = new List<CompareItem>();
                for (int i = 0; i < Request.Form.Files.Count - 1; i++)
                {
                    var file = Request.Form.Files[i];
                    string fileExtension = Path.GetExtension(file.FileName);
                    using (var sreader = new StreamReader(file.OpenReadStream()))
                    {
                        string[] headers = sreader.ReadLine().Split(',');
                        while (!sreader.EndOfStream)
                        {
                            var line = sreader.ReadLine();
                            if (!string.IsNullOrEmpty(line))
                            {
                                var item = line.Split("\t");
                                if (item[2].ToUpper() == "BUY CARD")
                                {
                                    lstCard.Add(new CompareItem()
                                    {
                                        TransDate = GetDateTime_IoMediaCsv(item[9]),
                                        TransCodePay = item[10],
                                        ReceivedAccount = string.Empty,
                                        Quantity = 1,
                                        ProviderValue = Convert.ToDecimal(item[3]),
                                        Amount = Convert.ToDecimal(item[8]),
                                        CompareDate = compareDate,
                                        ProviderCode = providerCode,
                                    });
                                }
                                else if (item[2].ToUpper() == "Billing")
                                {
                                    lstCard.Add(new CompareItem()
                                    {
                                        TransDate = GetDateTime_IoMediaCsv(item[9]),
                                        TransCodePay = item[10],
                                        ReceivedAccount = item[4],
                                        Quantity = 1,
                                        ProviderValue = Convert.ToDecimal(item[3]),
                                        Amount = Convert.ToDecimal(item[8]),
                                        CompareDate = compareDate,
                                        ProviderCode = providerCode,
                                    });
                                }
                                else
                                {
                                    lst.Add(new CompareItem()
                                    {
                                        TransDate = GetDateTime_IoMediaCsv(item[9]),
                                        TransCodePay = item[10],
                                        ReceivedAccount = item[4],
                                        Quantity = 1,
                                        ProviderValue = Convert.ToDecimal(item[3]),
                                        Amount = Convert.ToDecimal(item[8]),
                                        CompareDate = compareDate,
                                        ProviderCode = providerCode,
                                    });
                                }
                            }
                        }
                    }
                }

                var listGroupCard = (from x in lstCard
                                     group x by new { x.TransCodePay, x.ProviderCode, x.CompareDate } into g
                                     select new CompareItem()
                                     {
                                         TransDate = g.First().TransDate,
                                         TransCodePay = g.Key.TransCodePay,
                                         ReceivedAccount = string.Empty,
                                         Quantity = g.Count(),
                                         ProviderValue = g.Sum(c => c.ProviderValue),
                                         Amount = g.Sum(c => c.Amount),
                                         CompareDate = g.Key.CompareDate,
                                         ProviderCode = g.Key.ProviderCode,
                                     }).ToList();

                lst.AddRange(listGroupCard);
                return lst;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode} ReadFileIOMediaCsv error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return null;
            }

        }

        #endregion

        #region 6.IMedia .xls

        private async Task<List<CompareItem>> ReadFileIMedia(string providerCode)
        {
            try
            {
                List<CompareItem> lst = new List<CompareItem>();
                List<CompareItem> lstCard = new List<CompareItem>();
                //foreach (var fileItem in Request.Form.Files)
                for (int i = 0; i < Request.Form.Files.Count - 1; i++)
                {
                    var file = Request.Form.Files[i];
                    var desFileCsv = await GetFileCSV(file, providerCode, retry: 0);
                    var compareDate = DateTime.Now;
                    using (var sreader = System.IO.File.OpenRead(desFileCsv))
                    {
                        var lines = sreader.ReadLines();
                        bool isDowloadVpin = false;
                        foreach (var item in lines)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                if (item.Contains("DownloadSoftpin")) isDowloadVpin = true;
                                var line = item.Split('\t');
                                if (line.Length >= 10)
                                {
                                    if (isDowloadVpin)
                                    {
                                        var dateline = GetDateTime_Imedia(line[1]);
                                        lstCard.Add(new CompareItem()
                                        {
                                            TransDate = dateline,
                                            TransCodePay = line[5].Replace("'", ""),
                                            ReceivedAccount = line[8].Replace("'", ""),
                                            ProviderValue = Convert.ToDecimal(line[3].Replace(",", "")),
                                            Amount = Convert.ToDecimal(line[6].Replace(",", "")),
                                            CompareDate = compareDate,
                                            ProviderCode = providerCode,
                                        });
                                    }
                                    else
                                    if (line[7] == "Thanh Cong")
                                    {
                                        var dateline = GetDateTime_Imedia(line[1]);
                                        lst.Add(new CompareItem()
                                        {
                                            TransDate = dateline,
                                            TransCodePay = line[5].Replace("'", ""),
                                            ReceivedAccount = line[8].Replace("'", ""),
                                            ProviderValue = Convert.ToDecimal(line[3].Replace(",", "")),
                                            Amount = Convert.ToDecimal(line[4].Replace(",", "")),
                                            CompareDate = compareDate,
                                            ProviderCode = providerCode,
                                        });
                                    }
                                }
                                else
                                {
                                    line = item.Split(',');
                                    if (line.Length >= 9)
                                    {
                                        if (isDowloadVpin)
                                        {
                                            var dateline = GetDateTime_Imedia(line[1]);
                                            lstCard.Add(new CompareItem()
                                            {
                                                TransDate = dateline,
                                                TransCodePay = line[5].Replace("'", ""),
                                                ReceivedAccount = line[8].Replace("'", ""),
                                                ProviderValue = Convert.ToDecimal(line[3].Replace(",", "")),
                                                Amount = Convert.ToDecimal(line[6].Replace(",", "")),
                                                CompareDate = compareDate,
                                                ProviderCode = providerCode,
                                            });
                                        }
                                        else
                                        if (line[7] == "Thanh Cong")
                                        {
                                            var dateline = GetDateTime_Imedia2(line[1]);
                                            lst.Add(new CompareItem()
                                            {
                                                TransDate = dateline,
                                                TransCodePay = line[5].Replace("'", ""),
                                                ReceivedAccount = line[8].Replace("'", ""),
                                                ProviderValue = Convert.ToDecimal(line[3].Replace(".", "")),
                                                Amount = Convert.ToDecimal(line[4].Replace(".", "")),
                                                CompareDate = compareDate,
                                                ProviderCode = providerCode,
                                            });
                                        }
                                    }
                                }
                            }

                        }
                    }
                    System.IO.File.Delete(desFileCsv);
                }


                var listGroupCard = (from x in lstCard
                                     group x by new { x.TransCodePay, x.ProviderCode, x.CompareDate } into g
                                     select new CompareItem()
                                     {
                                         TransDate = g.First().TransDate,
                                         TransCodePay = g.Key.TransCodePay,
                                         ReceivedAccount = string.Empty,
                                         Quantity = g.Count(),
                                         ProviderValue = g.Sum(c => c.ProviderValue),
                                         Amount = g.Sum(c => c.Amount),
                                         CompareDate = g.Key.CompareDate,
                                         ProviderCode = g.Key.ProviderCode,
                                     }).ToList();

                lst.AddRange(listGroupCard);
                return lst;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode} ReadFileIMedia error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return null;
            }
        }

        private async Task<List<CompareItem>> ReadFileIMedia_V2(string providerCode)
        {
            try
            {
                List<CompareItem> lst = new List<CompareItem>();
                List<CompareItem> lstCard = new List<CompareItem>();
                for (int i = 0; i < Request.Form.Files.Count - 1; i++)
                {
                    var file = Request.Form.Files[i];
                    var desFileCsv = await GetFileCSV(file, providerCode, retry: 0);
                    var compareDate = DateTime.Now;
                    using (var sreader = System.IO.File.OpenRead(desFileCsv))
                    {
                        var lines = sreader.ReadLines();
                        bool isDowloadVpin = false;
                        if (file.FileName.Contains("Download"))
                            isDowloadVpin = true;
                        foreach (var item in lines)
                        {
                            if (!string.IsNullOrEmpty(item) && !item.StartsWith("STT"))
                            {
                                var line = item.Split(',');
                                if (line.Length >= 8)
                                {
                                    if (isDowloadVpin)
                                    {
                                        var dateline = GetDateTime_Imedia_V2(line[2]);
                                        lstCard.Add(new CompareItem()
                                        {
                                            TransDate = dateline,
                                            TransCodePay = line[1],
                                            ReceivedAccount = string.Empty,
                                            ProviderValue = Convert.ToDecimal(line[6])* Convert.ToInt32(line[4]),
                                            Amount = Convert.ToDecimal(line[6])* Convert.ToInt32(line[4]),
                                            Quantity = Convert.ToInt32(line[4]),
                                            CompareDate = compareDate,
                                            ProviderCode = providerCode,
                                        });
                                    }
                                    else
                                    {
                                        var dateline = GetDateTime_Imedia_V2(line[3]);
                                        lst.Add(new CompareItem()
                                        {
                                            TransDate = dateline,
                                            TransCodePay = line[1],
                                            ReceivedAccount = line[5],
                                            ProviderValue = Convert.ToDecimal(line[4]),
                                            Amount = Convert.ToDecimal(line[4]),
                                            Quantity = 1,
                                            CompareDate = compareDate,
                                            ProviderCode = providerCode,
                                        });
                                    }
                                }
                            }

                        }
                    }
                    System.IO.File.Delete(desFileCsv);
                }



                lst.AddRange(lstCard);
                return lst;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode} ReadFileIMedia error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return null;
            }

        }

        private async Task<List<CompareItem>> ReadFileIMedia2(string providerCode)
        {
            try
            {
                List<CompareItem> lst = new List<CompareItem>();
                List<CompareItem> lstCard = new List<CompareItem>();
                for (int i = 0; i < Request.Form.Files.Count - 1; i++)
                {
                    var file = Request.Form.Files[i];
                    var desFileCsv = await GetFileCSV(file, providerCode, retry: 0);
                    var compareDate = DateTime.Now;
                    using (var sreader = System.IO.File.OpenRead(desFileCsv))
                    {
                        var lines = sreader.ReadLines();
                        bool isDowloadVpin = false;
                        foreach (var item in lines)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                if (item.Contains("DownloadSoftpin")) isDowloadVpin = true;
                                var line = item.Split('\t');
                                if (line.Length >= 9)
                                {
                                    if (string.IsNullOrEmpty(line[1]) || line[1] == "Thoi Gian")
                                        continue;

                                    if (isDowloadVpin)
                                    {
                                        var dateline = GetDateTime_Imedia2(line[1]);
                                        lstCard.Add(new CompareItem()
                                        {
                                            TransDate = dateline,
                                            TransCodePay = line[5].Replace("'", ""),
                                            ReceivedAccount = "",
                                            ProviderValue = Convert.ToDecimal(line[3].Replace(",", "").Replace(".", "")),
                                            Amount = Convert.ToDecimal(line[6].Replace(",", "")),
                                            CompareDate = compareDate,
                                            ProviderCode = providerCode,
                                        });
                                    }
                                    else
                                    if (line[7] == "Thanh Cong")
                                    {
                                        var dateline = GetDateTime_Imedia2(line[1]);
                                        lst.Add(new CompareItem()
                                        {
                                            TransDate = dateline,
                                            TransCodePay = line[5].Replace("'", ""),
                                            ReceivedAccount = line[8].Replace("'", ""),
                                            ProviderValue = Convert.ToDecimal(line[3].Replace(",", "")),
                                            Amount = Convert.ToDecimal(line[4].Replace(",", "")),
                                            CompareDate = compareDate,
                                            ProviderCode = providerCode,
                                        });
                                    }
                                }
                            }

                        }
                    }
                    System.IO.File.Delete(desFileCsv);
                }


                var listGroupCard = (from x in lstCard
                                     group x by new { x.TransCodePay, x.ProviderCode, x.CompareDate } into g
                                     select new CompareItem()
                                     {
                                         TransDate = g.First().TransDate,
                                         TransCodePay = g.Key.TransCodePay,
                                         ReceivedAccount = string.Empty,
                                         Quantity = g.Count(),
                                         ProviderValue = g.Sum(c => c.ProviderValue),
                                         Amount = g.Sum(c => c.Amount),
                                         CompareDate = g.Key.CompareDate,
                                         ProviderCode = g.Key.ProviderCode,
                                     }).ToList();

                lst.AddRange(listGroupCard);
                return lst;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode} ReadFileIMedia error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return null;
            }

        }

        #endregion

        #region 7.123Card
        private async Task<List<CompareItem>> ReadFile123Card(string providerCode)
        {
            try
            {

                List<CompareItem> lst = new List<CompareItem>();
                //foreach (var fileItem in Request.Form.Files)
                for (int i = 0; i < Request.Form.Files.Count - 1; i++)
                {
                    var file = Request.Form.Files[i];
                    var desFileCsv = await GetFileCSV(file, providerCode, retry: 0);
                    var compareDate = DateTime.Now;
                    using (var sreader = System.IO.File.OpenRead(desFileCsv))
                    {
                        var lines = sreader.ReadLines();
                        foreach (var item in lines)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                var line = item.Replace('"', ' ').Split(',');
                                if (line.Length >= 8 && line[7] == "Thành công")
                                {
                                    lst.Add(new CompareItem()
                                    {
                                        TransDate = DateTime.Parse(line[8]),
                                        TransCodePay = line[0],
                                        ReceivedAccount = line[1],
                                        ProviderValue = Convert.ToDecimal(line[6]),
                                        Amount = Convert.ToDecimal(line[4]),
                                        CompareDate = compareDate,
                                        ProviderCode = providerCode,
                                    });
                                }
                            }

                        }
                    }
                    System.IO.File.Delete(desFileCsv);
                }
                return lst;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode} ReadFileMTC error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return null;
            }

        }

        #endregion

        #region 8.Kpp .csv

        private List<CompareItem> ReadFileKpp(string providerCode)
        {
            try
            {
                var compareDate = DateTime.Now;
                List<CompareItem> lst = new List<CompareItem>();
                //var fileItem in Request.Form.Files.Count - 1
                for (int i = 0; i < Request.Form.Files.Count - 1; i++)
                {
                    var file = Request.Form.Files[i];
                    string fileExtension = Path.GetExtension(file.FileName);
                    using (var sreader = new StreamReader(file.OpenReadStream()))
                    {
                        string[] headers = sreader.ReadLine().Split(',');
                        while (!sreader.EndOfStream)
                        {
                            var line = sreader.ReadLine();
                            if (!string.IsNullOrEmpty(line))
                            {
                                var item = line.Split(",");
                                if (item[9] == "1")
                                {
                                    lst.Add(new CompareItem()
                                    {
                                        TransDate = GetDateTime_Kpp(item[7]),
                                        TransCodePay = item[1],
                                        ReceivedAccount = item[5],
                                        ProviderValue = Convert.ToDecimal(item[3]),
                                        Amount = Convert.ToDecimal(item[4]),
                                        CompareDate = compareDate,
                                        ProviderCode = providerCode,
                                    });
                                }
                            }
                        }
                    }
                }

                return lst;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode} ReadFileKpp error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return null;
            }

        }

        #endregion

        #region 9.Vimo

        private async Task<List<CompareItem>> ReadFileVimo(string providerCode)
        {
            try
            {
                List<CompareItem> lst = new List<CompareItem>();
                for (int i = 0; i < Request.Form.Files.Count - 1; i++)
                {
                    var file = Request.Form.Files[i];
                    var desFileCsv = await GetFileCSV(file, providerCode, retry: 0);
                    var compareDate = DateTime.Now;
                    int index = 0;
                    using (var sreader = System.IO.File.OpenRead(desFileCsv))
                    {
                        var lines = sreader.ReadLines();
                        foreach (var item in lines)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                var line = item.Split(',');
                                if (line.Length == 13 && index > 0)
                                {
                                    if (line[12] == "Thành công")
                                    {
                                        lst.Add(new CompareItem()
                                        {
                                            TransDate = GetDateTime_Vimo(line[0], line[1]),
                                            TransCodePay = line[2],
                                            ReceivedAccount = line[6],
                                            ProviderValue = Convert.ToDecimal(line[7]),
                                            Amount = Convert.ToDecimal(line[9]),
                                            CompareDate = compareDate,
                                            ProviderCode = providerCode,
                                        });
                                    }
                                }
                                else if (line.Length == 14 && index > 0)
                                {
                                    if (line[13] == "Thành công")
                                    {
                                        if (line[5].StartsWith("Thanh"))
                                        {
                                            lst.Add(new CompareItem()
                                            {
                                                TransDate = GetDateTime_Vimo(line[0], line[1]),
                                                TransCodePay = line[2],
                                                ReceivedAccount = line[4],
                                                ProviderValue = Convert.ToDecimal(line[7]),
                                                Amount = Convert.ToDecimal(line[10]),
                                                CompareDate = compareDate,
                                                ProviderCode = providerCode,
                                            });
                                        }
                                        else
                                        {
                                            lst.Add(new CompareItem()
                                            {
                                                TransDate = GetDateTime_Vimo(line[0], line[1]),
                                                TransCodePay = line[2],
                                                ReceivedAccount = line[6],
                                                ProviderValue = Convert.ToDecimal(line[7]),
                                                Amount = Convert.ToDecimal(line[10]),
                                                CompareDate = compareDate,
                                                ProviderCode = providerCode,
                                            });
                                        }
                                    }
                                }
                                index += 1;
                            }

                        }
                    }
                    System.IO.File.Delete(desFileCsv);
                }
                return lst;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode} ReadFileIOMedia error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return null;
            }

        }

        #endregion

        #region 10.Payoo .csv
        private List<CompareItem> ReadFilePayooCsv(string providerCode)
        {
            try
            {

                var compareDate = DateTime.Now;
                List<CompareItem> lst = new List<CompareItem>();
                List<CompareItem> lstCard = new List<CompareItem>();
                for (int i = 0; i < Request.Form.Files.Count - 1; i++)
                {
                    var file = Request.Form.Files[i];
                    string fileExtension = Path.GetExtension(file.FileName);
                    using (var sreader = new StreamReader(file.OpenReadStream()))
                    {
                        string[] headers = sreader.ReadLine().Split(',');
                        while (!sreader.EndOfStream)
                        {
                            var line = sreader.ReadLine();
                            if (!string.IsNullOrEmpty(line))
                            {
                                var item = line.Split("\t");
                                if (item.Length > 1 && item[1] != null)
                                {
                                    if (item[5].ToUpper() == "PAYCODE")
                                    {
                                        lstCard.Add(new CompareItem()
                                        {
                                            TransDate = GetDateTime_PayooCsv(item[4]),
                                            TransCodePay = item[20].Replace("'", ""),
                                            ReceivedAccount = string.Empty,
                                            Quantity = int.Parse(item[9]),
                                            ProviderValue = Convert.ToDecimal(item[12]),
                                            Amount = Convert.ToDecimal(item[27]),
                                            CompareDate = compareDate,
                                            ProviderCode = providerCode,
                                        });
                                    }
                                    else
                                    {
                                        lst.Add(new CompareItem()
                                        {
                                            TransDate = GetDateTime_PayooCsv(item[4]),
                                            TransCodePay = item[20].Replace("'", ""),
                                            ReceivedAccount = item[8].Replace("'", ""),
                                            Quantity = 1,
                                            ProviderValue = Convert.ToDecimal(item[12]),
                                            Amount = Convert.ToDecimal(item[27]),
                                            CompareDate = compareDate,
                                            ProviderCode = providerCode,
                                        });
                                    }
                                }
                            }
                        }
                    }
                }

                var listGroupCard = (from x in lstCard
                                     group x by new { x.TransCodePay, x.ProviderCode, x.CompareDate } into g
                                     select new CompareItem()
                                     {
                                         TransDate = g.First().TransDate,
                                         TransCodePay = g.Key.TransCodePay,
                                         ReceivedAccount = string.Empty,
                                         Quantity = g.Count(),
                                         ProviderValue = g.Sum(c => c.ProviderValue * c.Quantity),
                                         Amount = g.Sum(c => c.Amount * c.Quantity),
                                         CompareDate = g.Key.CompareDate,
                                         ProviderCode = g.Key.ProviderCode,
                                     }).ToList();

                lst.AddRange(listGroupCard);
                var a = lst.Sum(x => x.ProviderValue);
                return lst;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode} ReadFilePayooCsv error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return null;
            }

        }
        #endregion

        #region 11.PayTech .xls

        private async Task<List<CompareItem>> ReadFilePayTech(string providerCode)
        {
            try
            {
                List<CompareItem> lst = new List<CompareItem>();
                for (int i = 0; i < Request.Form.Files.Count - 1; i++)
                {
                    var file = Request.Form.Files[i];
                    var desFileCsv = await GetFileCSV(file, providerCode, retry: 0);
                    var compareDate = DateTime.Now;
                    using (var sreader = System.IO.File.OpenRead(desFileCsv))
                    {
                        var lines = sreader.ReadLines();
                        foreach (var item in lines)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                var line = item.Split(',');
                                if (line.Length >= 16)
                                {
                                    if (line[13] == "Thành công")
                                    {
                                        lst.Add(new CompareItem()
                                        {
                                            TransDate = GetDateTime_PayTech(line[15]),
                                            TransCodePay = line[1].Split('_').Length >= 2 ? line[1].Split('_')[1] : line[1],
                                            ReceivedAccount = line[5].Contains("Nạp điện thoại") && !line[6].StartsWith("0") ? "0" + line[6] : line[6],
                                            ProviderValue = Convert.ToDecimal(line[10].Replace(".", "")),
                                            Amount = Convert.ToDecimal(line[11].Replace(".", "")),
                                            CompareDate = compareDate,
                                            ProviderCode = providerCode,
                                        });
                                    }
                                    else if (line[14] == "Thành công" && line.Length >= 17)
                                    {
                                        lst.Add(new CompareItem()
                                        {
                                            TransDate = GetDateTime_PayTech(line[16]),
                                            TransCodePay = line[2].Split('_').Length >= 2 ? line[2].Split('_')[1] : line[2],
                                            ReceivedAccount = line[6].Contains("Nạp điện thoại") && !line[7].StartsWith("0") ? "0" + line[7] : line[7],
                                            ProviderValue = Convert.ToDecimal(line[11].Replace(".", "")),
                                            Amount = Convert.ToDecimal(line[12].Replace(".", "")),
                                            CompareDate = compareDate,
                                            ProviderCode = providerCode,
                                        });
                                    }
                                }
                            }
                        }
                    }
                    System.IO.File.Delete(desFileCsv);
                }
                return lst;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode} ReadFilePayTech error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return null;
            }

        }
        #endregion

        #region 12.ESale .csv
        private List<CompareItem> ReadFileESaleCsv(string providerCode)
        {
            try
            {
                var compareDate = DateTime.Now;
                List<CompareItem> lst = new List<CompareItem>();
                List<CompareItem> lstCard = new List<CompareItem>();
                for (int i = 0; i < Request.Form.Files.Count - 1; i++)
                {
                    var file = Request.Form.Files[i];
                    string fileExtension = Path.GetExtension(file.FileName);
                    using (var sreader = new StreamReader(file.OpenReadStream()))
                    {
                        string[] headers = sreader.ReadLine().Split(',');
                        while (!sreader.EndOfStream)
                        {
                            var line = sreader.ReadLine();
                            if (!string.IsNullOrEmpty(line))
                            {
                                var item = line.Split("\t");
                                if (item.Length > 8 && item[1] != null)
                                {
                                    lstCard.Add(new CompareItem()
                                    {
                                        TransDate = GetDateTime_ESaleCsv(item[1]),
                                        TransCodePay = item[8].Replace("'", ""),
                                        ReceivedAccount = string.Empty,
                                        Quantity = int.Parse(item[4]),
                                        ProviderValue = 0,
                                        Amount = Convert.ToDecimal(item[5].Replace(".", "")),
                                        CompareDate = compareDate,
                                        ProviderCode = providerCode,
                                    });
                                }
                            }
                        }
                    }
                }
                lst.AddRange(lstCard);
                var a = lst.Sum(x => x.ProviderValue);
                return lst;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode} ReadFileESaleCsv error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return null;
            }

        }
        #endregion

        #region 13.Time Day

        private DateTime GetDateTime_NT(string dateTime)
        {

            var createdDate = new DateTime(Convert.ToInt32(dateTime.Substring(0, 4)), Convert.ToInt32(dateTime.Substring(4, 2)),
                                       Convert.ToInt32(dateTime.Substring(6, 2)), Convert.ToInt32(dateTime.Substring(8, 2)),
                                       Convert.ToInt32(dateTime.Substring(10, 2)), Convert.ToInt32(dateTime.Substring(12, 2)));
            return createdDate;

        }

        private DateTime GetDateTimeFile_NT(string dateTime)
        {
            try
            {
                var createdDate = new DateTime(Convert.ToInt32(dateTime.Substring(0, 4)), Convert.ToInt32(dateTime.Substring(4, 2)),
                                           Convert.ToInt32(dateTime.Substring(6, 2)));
                return createdDate;
            }
            catch (Exception ex)
            {
                return DateTime.Now;
            }

        }

        private DateTime GetDateTime_Octa(string dateTime)
        {
            string[] dateTimeArr = dateTime.Split(' ');
            string[] date = dateTimeArr[0].Split('-');
            string[] time = dateTimeArr[1].Split(':');

            var createdDate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]),
                                       Convert.ToInt32(date[0]), Convert.ToInt32(time[0]),
                                       Convert.ToInt32(time[1]), Convert.ToInt32(time[2]));
            return createdDate;

        }

        private DateTime GetDateTime_IoMediaCsv(string dateTime)
        {
            string[] dateTimeArr = dateTime.Split(' ');
            string[] date = dateTimeArr[0].Split('/');
            string[] time = dateTimeArr[1].Split(':');

            var createdDate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]),
                                       Convert.ToInt32(date[0]), Convert.ToInt32(time[0]),
                                       Convert.ToInt32(time[1]), Convert.ToInt32(time[2]));
            return createdDate;

        }

        private DateTime GetDateTime_Imedia(string dateTime)
        {
            string[] dateTimeArr = dateTime.Split(' ');
            string[] date = dateTimeArr[0].Split('/');
            string[] time = dateTimeArr[1].Split(':');

            var createdDate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]),
                                       Convert.ToInt32(date[0]), Convert.ToInt32(time[0]),
                                       Convert.ToInt32(time[1]), Convert.ToInt32(time[2]));
            return createdDate;

        }

        private DateTime GetDateTime_Imedia2(string dateTime)
        {
            string[] dateTimeArr = dateTime.Split(' ');
            string[] date = dateTimeArr[0].Split('/');
            string[] time = dateTimeArr[1].Split(':');

            var createdDate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]),
                                       Convert.ToInt32(date[0]), Convert.ToInt32(time[0]),
                                       Convert.ToInt32(time[1]), 0);
            return createdDate;

        }


        private DateTime GetDateTime_Imedia_V2(string dateTime)
        {
            string[] dateTimeArr = dateTime.Split(' ');
            string[] time = dateTimeArr[0].Split(':');
            string[] date = dateTimeArr[1].Split('-');
            var createdDate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]),
                                       Convert.ToInt32(date[0]), Convert.ToInt32(time[0]),
                                       Convert.ToInt32(time[1]), Convert.ToInt32(time[2]));
            return createdDate;

        }

        private DateTime GetDateTime_Vimo(string time, string day)
        {
            string[] d = time.Replace('"', ' ').Replace("\\", "").Split(':');
            string[] date = day.Replace('"', ' ').Replace("\\", "").Split('/');
            var createdDate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]),
                                       Convert.ToInt32(date[0]), Convert.ToInt32(d[0]),
                                       Convert.ToInt32(d[1]), 0);
            return createdDate;

        }

        private DateTime GetDateTime_PayTech(string day)
        {
            var str = day.Split(' ');
            string[] d = str[0].Split(':');
            string[] date = str[1].Split('/');
            var createdDate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]),
                                       Convert.ToInt32(date[0]), Convert.ToInt32(d[0]),
                                       Convert.ToInt32(d[1]), 0);
            return createdDate;

        }

        private async Task<string> GetFileCSV(Microsoft.AspNetCore.Http.IFormFile file, string providerCode, int retry = 0)
        {
            string pathSave = "";
            try
            {
                _logger.LogInformation($"{providerCode} Start GetFileCSV Copy file  : {file.FileName}");
                pathSave = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", providerCode + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls");
                using (var stream = System.IO.File.Create(pathSave))
                {
                    await file.CopyToAsync(stream);
                    stream.Close();
                }

                _logger.LogInformation($"{providerCode} GetFileCSV Copy xong file  : {file.FileName}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode} GetFileCSV CopyFIle .xls error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
            }

            _logger.LogInformation($"{providerCode} Continue GetFileCSV Copy xu ly xong file  : {file.FileName}");
            _logger.LogInformation($"{providerCode} Start GetFileCSV Save sang file  csv");
            try
            {
                var book = new Workbook(pathSave);
                _logger.LogInformation($"{providerCode} Continue GetFileCSV Save sang file csv khoi tao");
                var desFileCsv = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", providerCode + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv");
                if (System.IO.File.Exists(desFileCsv))
                    return desFileCsv;

                // save XLSX as CSV
                _logger.LogInformation($"{providerCode} Continue GetFileCSV Save sang file csv getPath");
                book.Save(desFileCsv, SaveFormat.CSV);
                _logger.LogInformation($"{providerCode} Continue GetFileCSV Save sang file csv Save file Done");
                System.IO.File.Delete(pathSave);

                _logger.LogInformation($"{providerCode} Continue GetFileCSV Save sang file csv xong: {desFileCsv}");
                return desFileCsv;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{providerCode} GetFileCSV Save .csv error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                if (retry >= 1)
                    return string.Empty;
                System.Threading.Thread.Sleep(1000);
                return await GetFileCSV(file, providerCode, retry = 1);
            }
        }

        private DateTime GetDateTime_Kpp(string dateTime)
        {
            string[] dateTimeArr = dateTime.Split(' ');
            string[] date = dateTimeArr[0].Split('-');
            string[] time = dateTimeArr[1].Split(':');

            var createdDate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]),
                                       Convert.ToInt32(date[0]), Convert.ToInt32(time[0]),
                                       Convert.ToInt32(time[1]), Convert.ToInt32(time[2]));
            return createdDate;

        }

        private string FileNameNCC()
        {
            List<string> _f = new List<string>();
            for (int i = 0; i < Request.Form.Files.Count - 1; i++)
            {
                var file = Request.Form.Files[i];
                _f.Add(file.FileName);
            }

            return string.Join(",", _f);

        }

        private DateTime GetDateTime_PayooCsv(string dateTime)
        {
            string[] dateTimeArr = dateTime.TrimStart('\'').Replace("'", "").Split(' ');
            string[] date = dateTimeArr[0].Split('/');
            string[] time = dateTimeArr[1].Split(':');

            var createdDate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]),
                                       Convert.ToInt32(date[0]), Convert.ToInt32(time[0]),
                                       Convert.ToInt32(time[1]), Convert.ToInt32(time[2]));
            return createdDate;

        }

        private DateTime GetDateTime_ESaleCsv(string dateTime)
        {
            string[] dateTimeArr = dateTime.TrimStart('\'').Replace("'", "").Split(' ');
            string[] date = dateTimeArr[0].Split('/');
            string[] time = dateTimeArr[1].Split(':');

            var createdDate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]),
                                       Convert.ToInt32(date[0]), Convert.ToInt32(time[0]),
                                       Convert.ToInt32(time[1]), Convert.ToInt32(time[2]));
            return createdDate;

        }

        #endregion

        #endregion
    }

}
