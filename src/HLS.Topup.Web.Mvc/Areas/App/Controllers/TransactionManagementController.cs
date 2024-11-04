using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.Transactions;
using HLS.Topup.Web.Areas.App.Models.TransactionManagement;
using HLS.Topup.Web.Areas.App.Models.Providers;
using HLS.Topup.Compare;
using System;
using Abp.UI;
using System.Collections.Generic;
using System.IO;
using ServiceStack;
using Aspose.Cells;
using Microsoft.Extensions.Logging;
using NPOI.POIFS.Crypt.Dsig;
using HLS.Topup.Common;
using Stripe;
using System.Linq;
using HLS.Topup.StockManagement;
using Hangfire;
using Abp.Runtime.Session;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_TransactionManagements)]
    public class TransactionManagementController : TopupControllerBase
    {
        private readonly ILogger<TransactionManagementController> _logger;
        private readonly ITransactionsAppService _transactionsAppService;
        public TransactionManagementController(ITransactionsAppService transactionsAppService,
            ILogger<TransactionManagementController> logger)
        {
            _logger = logger;
            _transactionsAppService = transactionsAppService;
        }

        public async Task<ActionResult> Index()
        {
            var viewModel = new TransactionManagementViewModel()
            {
                ProviderList = await _transactionsAppService.GetAllProviderForTableDropdown(),
                TransactionsServiceList = await _transactionsAppService.GetAllServiceForTableDropdown()
            };

            return View(viewModel);
        }

        public async Task<PartialViewResult> PinCodeDetailModal(string transCode)
        {
            var viewModel = new TransactionDetailViewDto()
            {
                TransCode = transCode
            };

            return PartialView("_PinCodeDetailModal", viewModel);
        }

        public async Task<ActionResult> OffsetTopup()
        {
            return View();
        }

        public async Task<IActionResult> CreateImportModal()
        {
            return PartialView("_CreateImportModal");
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TransactionManagements_StatusFile)]
        public async Task<ResponseMessages> ReadFileData()
        {
            try
            {
                var list = new List<TransItemDto>();
                var fileData = Request.Form.Files[0];
                if (fileData == null)
                {
                    return new ResponseMessages
                    {
                        ResponseCode = "00",
                        ResponseMessage = "Quý khách chưa chọn file để import"
                    };
                }
                else
                {
                    var extension = Path.GetExtension(Request.Form.Files[0].FileName).ToLower();
                    if (extension == ".xls" || extension == ".xlsx" || extension == ".xlsm")
                    {

                        if (fileData.Length > 1048576 * 100 || fileData.Length > 1048576 * 100)
                        {
                            throw new UserFriendlyException(L("File_SizeLimit_Error"));
                        }

                        list = await ReadFileExls();
                        return new ResponseMessages
                        {
                            ResponseCode = "01",
                            ResponseMessage = "Thành công",
                            Payload = list
                        };
                    }
                    else
                    {
                        return new ResponseMessages
                        {
                            ResponseCode = "00",
                            ResponseMessage = "Định dạng file không hợp lệ."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ReadFileCompare error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return new ResponseMessages
                {
                    ResponseCode = "00",
                    ResponseMessage = L("Error")
                };
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TransactionManagements_StatusFile)]
        public async Task<ResponseMessages> ProcessFromJobExcel()
        {
            try
            {
                var list = new List<TransItemDto>();
                var fileData = Request.Form.Files[0];
                if (fileData == null)
                {
                    return new ResponseMessages
                    {
                        ResponseCode = "00",
                        ResponseMessage = "Quý khách chưa chọn file để import"
                    };
                }
                else
                {
                    var extension = Path.GetExtension(Request.Form.Files[0].FileName).ToLower();
                    if (extension == ".xls" || extension == ".xlsx" || extension == ".xlsm")
                    {

                        if (fileData.Length > 1048576 * 100 || fileData.Length > 1048576 * 100)
                        {
                            throw new UserFriendlyException(L("File_SizeLimit_Error"));
                        }

                        list = await ReadFileExls();
                        var data = list.ToJson();
                        BackgroundJob.Enqueue<TransactionsAppService>((x) => x.ProcessSyncStatusTransactionJob(AbpSession.ToUserIdentifier(), data));                        
                        return new ResponseMessages
                        {
                            ResponseCode = "01",
                            ResponseMessage = "Thành công",
                            Payload = list
                        };
                    }
                    else
                    {
                        return new ResponseMessages
                        {
                            ResponseCode = "00",
                            ResponseMessage = "Định dạng file không hợp lệ."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProcessFromJobExcel error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return new ResponseMessages
                {
                    ResponseCode = "00",
                    ResponseMessage = L("Error")
                };
            }
        }

        private async Task<List<TransItemDto>> ReadFileExls()
        {
            try
            {
                List<TransItemDto> lst = new List<TransItemDto>();
                for (int i = 0; i < Request.Form.Files.Count; i++)
                {
                    var file = Request.Form.Files[i];
                    var desFileCsv = await GetFileCSV(file, retry: 0);
                    var compareDate = DateTime.Now;
                    using (var sreader = System.IO.File.OpenRead(desFileCsv))
                    {
                        var lines = sreader.ReadLines();
                        foreach (var item in lines)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                var line = item.Split(',');
                                if (line.Length >= 3 && line[0] != "STT")
                                {
                                    if (!string.IsNullOrEmpty(line[1]) && !string.IsNullOrEmpty(line[2]))
                                    {
                                        lst.Add(new TransItemDto()
                                        {
                                            TransCode = line[1],
                                            StatusName = getStatusNow(line[2]),
                                            Status = getStatusNow(line[2], isName: false)
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
                _logger.LogError($"ReadFileExls error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return null;
            }

        }

        private async Task<string> GetFileCSV(Microsoft.AspNetCore.Http.IFormFile file, int retry = 0)
        {
            string pathSave = "";
            try
            {
                _logger.LogInformation($"Start GetFileCSV Copy file  : {file.FileName}");
                pathSave = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", "Data_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls");
                using (var stream = System.IO.File.Create(pathSave))
                {
                    await file.CopyToAsync(stream);
                    stream.Close();
                }

                _logger.LogInformation($"GetFileCSV Copy xong file  : {file.FileName}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetFileCSV CopyFIle .xls error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
            }

            _logger.LogInformation($"Continue GetFileCSV Copy xu ly xong file  : {file.FileName}");
            _logger.LogInformation($"Start GetFileCSV Save sang file  csv");
            try
            {
                var book = new Workbook(pathSave);
                _logger.LogInformation($"Continue GetFileCSV Save sang file csv khoi tao");
                var desFileCsv = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", "Data_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv");
                if (System.IO.File.Exists(desFileCsv))
                    return desFileCsv;

                // save XLSX as CSV
                _logger.LogInformation($"Continue GetFileCSV Save sang file csv getPath");
                book.Save(desFileCsv, SaveFormat.CSV);
                _logger.LogInformation($"Continue GetFileCSV Save sang file csv Save file Done");
                System.IO.File.Delete(pathSave);

                _logger.LogInformation($"Continue GetFileCSV Save sang file csv xong: {desFileCsv}");
                return desFileCsv;
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetFileCSV Save .csv error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                if (retry >= 1)
                    return string.Empty;
                System.Threading.Thread.Sleep(1000);
                return await GetFileCSV(file, retry = 1);
            }
        }

        private string getStatusNow(string status, bool isName = true)
        {
            string str = (status ?? string.Empty).ToLower().TrimEnd().TrimStart();
            string a = "";
            for (int i = 0; i < str.Length; i++)
            {
                a = a + getChar(str[i]);
            }

            if (status == "1" || a == "thanh cong")
                return isName ? "Thành công" : "1";
            else if (status == "2" || a == "huy")
                return isName ? "Hủy" : "2";
            else if (status == "3" || a == "loi")
                return isName ? "Lỗi" : "3";
            else if (status == "4" || a == "qua thoi gian")
                return isName ? "Quá thời gian" : "4";
            else if (status == "6" || a == "dang xu ly")
                return isName ? "Đang xử lý" : "6";
            else if (status == "7" || a == "timeout")
                return isName ? "Timeout" : "7";
            else if (status == "8" || a == "chua co ket qua")
                return isName ? "Chưa có kết quả" : "8";
            else if (status == "9" || a == "da thanh toan")
                return isName ? "Đã thanh toán" : "9";
            else if (status == "20" || a == "huy gd - hoan tien")
                return isName ? "Hủy GD - Hoàn tiền" : "20";
            else return "";
        }

        private string getChar(char c)
        {
            if ("àáảãạâầấẩẫậăằắẳẵặ".Contains(c))
                return "a";
            else if ("òóỏõọơờớởỡợôồốổỗộ".Contains(c))
                return "o";
            else if ("ùúủũụưừứửữự".Contains(c))
                return "u";
            else if ("ìíỉị".Contains(c))
                return "i";
            else if ("yỳýỷỹỵ".Contains(c))
                return "y";
            else if ("èéẻẽẹêềếểễệ".Contains(c))
                return "e";
            else if ("đ".Contains(c))
                return "d";
            return c.ToString();
        }

    }
}
