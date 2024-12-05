using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Hangfire;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Bill;
using HLS.Topup.Dtos.Transactions;
using HLS.Topup.Products;
using HLS.Topup.Report;
using HLS.Topup.Reports.Dtos;
using HLS.Topup.Reports.Exporting;
using HLS.Topup.RequestDtos;
using HLS.Topup.Transactions;
using Microsoft.Extensions.Logging;
using ServiceStack;
using static HLS.Topup.Report.ReportComparePartnerExportInfo;

namespace HLS.Topup.Reports
{
    public partial class ReportSystemAppService
    {
        #region I.Read Excel Báo cáo chi tiết bán hàng BackEnd

        private async Task<FileDto> ExportFileSaleServiceData(GetReportServiceDetailInput input)
        {
            var request = input.ConvertTo<ReportServiceDetailRequest>();
            request.Limit = 1;
            request.Offset = 0;
            request.SearchType = SearchType.Search;

            request.AccountType = (int)_session.AccountType;
            request.LoginCode = _session.AccountCode;
            var check = await _reportsManager.ReportServiceDetailReport(request);
            request.Limit = int.MaxValue;
            if (check.Total >= 5000)
            {
                var lst = new List<ReportServiceDetailDto>();
                var lstDate = GetDateFile("REPORT.TRANS", input.FromDate.Value, input.ToDate.Value);
                Parallel.ForEach(lstDate, file =>
                 {
                     var data = ReadFileSaleServiceData(file);
                     lst.AddRange(data);
                 });
                return _excelExporter.ReportServiceDetailExportToFile(lst);
            }
            else
            {
                var rs = await _reportsManager.ReportServiceDetailReport(request);
                var lst = rs.Payload.ConvertTo<List<ReportServiceDetailDto>>();
                return _excelExporter.ReportServiceDetailExportToFile(lst);
            }
        }
        private List<ReportServiceDetailDto> ReadFileSaleServiceData(string linkFile)
        {
            try
            {
                var lst = new List<ReportItemDetailDto>();
                var list = new List<ReportServiceDetailDto>();
                using (var sreader = File.OpenRead(linkFile))
                {
                    var lines = sreader.ReadLines();
                    foreach (var item in lines)
                    {
                        var old = string.Empty;
                        try
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                if (item.StartsWith("STT"))
                                    continue;

                                old = item;
                                var str = item.Split(',');
                                lst.Add(new ReportItemDetailDto()
                                {
                                    PerformAccount = str[1],
                                    PerformInfo = str[2] + "-" + str[3],
                                    AccountCode = str[5],
                                    AccountInfo = str[6] + "-" + str[7],
                                    AccountAgentType = Convert.ToInt32(str[8]),
                                    AccountAgentName = str[9],
                                    ServiceCode = str[10],
                                    ServiceName = str[11],
                                    TransType = str[12],
                                    ProductCode = str[13],
                                    ProductName = str[14],
                                    CategoryCode = str[15],
                                    CategoryName = str[16],
                                    CreatedTime = Convert.ToDateTime(str[17]),
                                    RequestRef = str[18],
                                    TransCode = str[19],
                                    PaidTransCode = str[20],
                                    PayTransRef = str[21],
                                    Status = str[22] == "1" ? ReportStatus.Success : str[22] == "3" ? ReportStatus.Error : ReportStatus.TimeOut,
                                    Quantity = Convert.ToInt32(str[23]),
                                    Amount = Convert.ToDouble(str[24]),
                                    Fee = Convert.ToDouble(str[25]),
                                    Discount = Convert.ToDouble(str[26]),
                                    Price = Convert.ToDouble(str[27]),
                                    Balance = Convert.ToDouble(str[28]),
                                    PerformBalance = Convert.ToDouble(str[29]),
                                    ReceivedAccount = str[30],
                                    ProvidersCode = str[32],
                                });
                            }

                        }
                        catch (Exception e)
                        {

                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ReadFileData: {linkFile} error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return new List<ReportServiceDetailDto>();
            }

        }

        #endregion

        #region II.Read Excel Báo cáo Lịch sử số dư Backend

        private async Task<FileDto> ExportFileBalanceHistoryData(ReportDetailRequest input)
        {
            var lst = new List<ReportDetailDto>();
            var lstDate = GetDateFile("Report.BalanceHistory", input.FromDate.Value, input.ToDate.Value);
            Parallel.ForEach(lstDate, file =>
            {
                var data = ReadFileBalanceHistoryData(file, input.AccountCode);
                lst.AddRange(data);
            });

            return _excelExporter.ReportDetailExportToFile(lst);
        }
        private List<ReportDetailDto> ReadFileBalanceHistoryData(string linkFile, string accountCode)
        {
            try
            {
                var lst = new List<ReportDetailDto>();
                using (var sreader = File.OpenRead(linkFile))
                {
                    var lines = sreader.ReadLines();
                    foreach (var item in lines)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                if (item.StartsWith("STT"))
                                    continue;
                                //STT,AccountCode,AccountName,Mobile,FullName,BalanceBefore,AmountUp,AmountDown,BalanceAfter,CreatedDate,ServiceCode,ServiceName,TransCode,TransNote,Description
                                var str = item.Split(',');
                                if (str[1] == accountCode)
                                {
                                    lst.Add(new ReportDetailDto()
                                    {
                                        TransCode = str[12],
                                        CreatedDate = Convert.ToDateTime(str[9]),
                                        Increment = Convert.ToDecimal(str[6]),
                                        Decrement = Convert.ToDecimal(str[7]),
                                        BalanceBefore = Convert.ToDecimal(str[5]),
                                        BalanceAfter = Convert.ToDecimal(str[8]),
                                        ServiceCode = str[10],
                                        ServiceName = str[11],
                                        TransNote = str[13],
                                        Description = str[14],
                                    });
                                }
                            }

                        }
                        catch (Exception e)
                        {

                        }
                    }
                }

                return lst;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ReadFileBalanceHistoryData: {linkFile} error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return new List<ReportDetailDto>();
            }

        }

        #endregion

        #region III.Read Excel Báo cáo Lịch sử số dư Fontend

        private async Task<FileDto> ExportFileTransDetailData(ReporttransDetailRequest input)
        {
            var lst = new List<ReportTransDetailDto>();
            var lstDate = GetDateFile("REPORT.TRANS", input.FromDate.Value, input.ToDate.Value);
            Parallel.ForEach(lstDate, file =>
            {
                var data = ReadFileTransDetailData(file, input.AccountCode);
                lst.AddRange(data);
            });
            return _excelExporter.ReportTransDetailExportToFile(lst);
        }
        private List<ReportTransDetailDto> ReadFileTransDetailData(string linkFile, string accountCode)
        {
            try
            {
                var lst = new List<ReportTransDetailDto>();
                using (var sreader = File.OpenRead(linkFile))
                {
                    var lines = sreader.ReadLines();
                    foreach (var item in lines)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                if (item.StartsWith("STT"))
                                    continue;

                                var str = item.Split(',');
                                lst.Add(new ReportTransDetailDto()
                                {
                                    //PerformAccount = str[1],
                                    //PerformAccountItem = new AccountItemDto()
                                    //{
                                    //    Mobile = str[2],
                                    //    FullName = str[3],
                                    //    AgentType = !string.IsNullOrEmpty(str[4]) ? Convert.ToInt32(str[4]) : 0,
                                    //},
                                    //AccountCode = str[5],
                                    //AccountItem = new AccountItemDto()
                                    //{
                                    //    Mobile = str[6],
                                    //    FullName = str[7],
                                    //    AgentType = Convert.ToInt32(str[8]),
                                    //    AgentName = str[9],
                                    //},
                                    //ServiceCode = str[10],
                                    //ServiceItem = new ServiceItemDto()
                                    //{
                                    //    ServiceCode = str[10],
                                    //    ServicesName = str[11],
                                    //},
                                    //TransType = str[12],
                                    //ProductCode = str[13],
                                    //ProductItem = new ProductItemDto()
                                    //{
                                    //    ProductName = str[14],
                                    //    CategoryCode = str[15],
                                    //    CategoryName = str[16],
                                    //},
                                    //CreatedTime = Convert.ToDateTime(str[17]),
                                    //RequestRef = str[18],
                                    //TransCode = str[19],
                                    //PaidTransCode = str[20],
                                    //PayTransRef = str[21],
                                    //Status = str[22] == "1" ? ReportStatus.Success : str[22] == "3" ? ReportStatus.Error : ReportStatus.TimeOut,
                                    //Quantity = Convert.ToInt32(str[23]),
                                    //Amount = Convert.ToDecimal(str[24]),
                                    //Fee = Convert.ToDecimal(str[25]),
                                    //Discount = Convert.ToDecimal(str[26]),
                                    //Price = Convert.ToDecimal(str[27]),
                                    //Balance = Convert.ToDecimal(str[28]),
                                    //PerformBalance = Convert.ToDecimal(str[29]),
                                    //ReceivedAccount = str[30],
                                    //ProvidersCode = str[32],
                                });
                            }

                        }
                        catch (Exception e)
                        {

                        }
                    }
                }

                return lst;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ReadFileData: {linkFile} error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return new List<ReportTransDetailDto>();
            }

        }

        #endregion

        private List<string> GetDateFile(string nameFile, DateTime fromDate, DateTime todate)
        {
            var f = new List<string>();
            var tmpDate = fromDate.Date;
            while (tmpDate <= todate.Date)
            {
                f.Add($"{nameFile}.{tmpDate.ToString("yyyyMMdd")}.csv");
                tmpDate = tmpDate.AddDays(1);
            }
            return f;
        }

        //private bool CheckFileSouce(string souceFileName)
        //{
        //    GetRootSouceFile();
        //}

        private string GetRootSouceFile()
        {
            var souce = _appConfiguration["App:RootSouceFileReport"];
            return souce;
        }

        public async Task<ValidateSearch> CheckValidateSearch(ValidateSearchInput input)
        {
            string msg = "";
            var enable = IsValidateSearch(input, ref msg);
            var dto = new ValidateSearch()
            {
                Code = enable ? "1" : "0",
                Message = msg
            };

            return dto;
        }

        private bool IsValidateSearch(ValidateSearchInput input, ref string msg)
        {
            try
            {
                var enable = _appConfiguration["ValidateSearch:Enable"];
                if (enable == null || !Convert.ToBoolean(enable))
                    return true;

                if (input.FromDate > input.ToDate)
                {
                    msg = input.Type.ToUpper() == "EXPORT"
                        ? "Thời gian kết xuất dữ liệu từ đang lớn hơn thời gian tới. Xin vui lòng chọn thời gian phù hợp để kết xuất"
                        : "Thời gian tìm kiếm từ đang lớn hơn thời gian tới. Xin vui lòng chọn thời gian tìm kiếm cho phù hợp";
                    return false;
                }

                var timeSpan = input.ToDate.Date - input.FromDate.Date;
                var day = input.ReportType.ToUpper() == "TOTAL"
                       ? Convert.ToInt32(_appConfiguration["ValidateSearch:DayTotal"])
                       : Convert.ToInt32(_appConfiguration["ValidateSearch:DayDetail"]);

                if (timeSpan.TotalDays > day)
                {
                    msg = input.Type.ToUpper() == "EXPORT" ? $"Thời gian kết xuất dữ liệu của quý khách vượt quá {day} ngày. Xin vui lòng chọn khoảng thời gian ngắn hơn để kết xuất dữ liệu."
                        : $"Thời gian tìm kiếm của quý khách vượt quá {day} ngày. Xin vui lòng chọn khoảng thời gian tìm kiếm ngắn hơn.";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        private bool IsValidateExport(ValidateSearchInput input, ref FileDto msg)
        {
            var smg = string.Empty;
            if (!IsValidateSearch(input, ref smg))
            {
                msg = new FileDto()
                {
                    FileName = "Warning",
                    FilePath = smg,
                };
                return false;
            }
            return true;
        }
    }
}
