using HLS.Topup.Common;
using HLS.Topup.Dto;
using HLS.Topup.Report;
using HLS.Topup.Reports.Dtos;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLS.Topup.Reports
{
    public partial class ReportSystemAppService
    {
        public async Task<bool> SendMailReportComparePartner(
         SendMailComparePartnerRequest request)
        {
            try
            {
                var rs = await _reportsManager.SendmailReportComparePartner(request);
                return true;

            }
            catch (Exception e)
            {
                _logger.LogError($"SendMailReportComparePartner error: {e}");
                return false;

            }
        }
        public async Task<PagedResultDtoReport<ReportComparePartnerDto>> GetReportComparePartnerList(
         GetReportComparePartnerInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportComparePartnerRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportComparePartnerDto>(0,
                    new ReportComparePartnerDto(),
                    new List<ReportComparePartnerDto>(), warning: msg);

                var rs = await _reportsManager.ReportComparePartner(request);
                var totalCount = rs.Total;

                var sumList = rs.SumData.ConvertTo<List<ReportComparePartnerDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportComparePartnerDto();

                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportComparePartnerDto>(0, new ReportComparePartnerDto(),
                        new List<ReportComparePartnerDto>());

                var lst = rs.Payload.ConvertTo<List<ReportComparePartnerDto>>();

                if (input.Type == "PAYBILL")
                {
                    sumData.DiscountVat = Math.Round(sumData.Discount / 11, 0);
                    sumData.DiscountNoVat = sumData.Discount - sumData.DiscountVat;
                }

                return new PagedResultDtoReport<ReportComparePartnerDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportServiceTotalList error: {e}");
                return new PagedResultDtoReport<ReportComparePartnerDto>(0,
                    new ReportComparePartnerDto(),
                    new List<ReportComparePartnerDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportBalancePartnerDto>> GetReportBalancePartner(
        GetReportBalancePartnerInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportComparePartnerRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportBalancePartnerDto>(0,
                    new ReportBalancePartnerDto(),
                    new List<ReportBalancePartnerDto>(), warning: msg);

                var rs = await _reportsManager.ReportBalancePartner(request);
                var totalCount = rs.Total;

                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportBalancePartnerDto>(0, new ReportBalancePartnerDto(),
                        new List<ReportBalancePartnerDto>());

                var lst = rs.Payload.ConvertTo<List<ReportBalancePartnerDto>>();

                lst.ForEach(c =>
                {
                    c.Price = c.Value;
                });

                return new PagedResultDtoReport<ReportBalancePartnerDto>(
                    totalCount, totalData: new ReportBalancePartnerDto(), lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportBalancePartner error: {e}");
                return new PagedResultDtoReport<ReportBalancePartnerDto>(
                    0,
                    new ReportBalancePartnerDto(),
                    new List<ReportBalancePartnerDto>());
            }
        }

        public async Task<FileDto> GetReportComparePartnerToExcel(GetReportComparePartnerInput input)
        {

            string msg = string.Empty;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var user = await UserManager.GetUserByAccountCodeAsync(input.AgentCode);
            var request = input.ConvertTo<ReportComparePartnerRequest>();
            request.Offset = 0;
            request.Limit = int.MaxValue;
            var profile = _profileRepository.FirstOrDefault(c => c.UserId == user.Id);
            var rs = await _reportsManager.ReportComparePartner(request);
            request.Type = "BALANCE";
            var rsBalance = await _reportsManager.ReportBalancePartner(request);

            var lst = rs.Payload.ConvertTo<List<ReportComparePartnerDto>>();
            var lstBalance = rsBalance.Payload.ConvertTo<List<ReportBalancePartnerDto>>();

            var partnerInput = new ReportComparePartnerExportInfo()
            {
                Title = "BIÊN BẢN DOANH THU DỊCH VỤ THANH TOÁN",
                PeriodCompare = string.Format("Kỳ đối soát: Từ: {0} - {1}",
                input.FromDate?.ToString("dd/MM/yyyy"), input.ToDate?.ToString("dd/MM/yyyy")),
                Contract = $"HỢP ĐỒNG SỐ: ......{(profile != null ? profile.ContractNumber : "")}....... Ký ngày:......{(profile != null ? profile.SigDate?.ToString("dd/MM/yyyy") : "")}.........",
                Provider = $"{input.AgentCode}",
                PeriodPayment = $"CHU KỲ : ...{(profile != null ? profile.PeriodCheck.ToString() : "")}.....",
                PinCodeItems = lst.Where(p => p.ServiceCode == "PIN_CODE").ToList(),
                PinGameItems = lst.Where(p => p.ServiceCode == "PIN_GAME").ToList(),
                TopupItems = lst.Where(p => p.ServiceCode == "TOPUP" && p.CategoryCode!="VTE_TOPUP").ToList(),
                TopupPrepaIdItems = lst.Where(p => p.ServiceCode == "TOPUP" && p.ReceiverType == "PREPAID").ToList(),
                TopupPostpaIdItems = lst.Where(p => p.ServiceCode == "TOPUP" && p.ReceiverType == "POSTPAID").ToList(),
                DataItems = lst.Where(p => p.ServiceCode == "PIN_DATA" || p.ServiceCode == "TOPUP_DATA").ToList(),
                PayBillItems = lst.Where(p => p.ServiceCode == "PAY_BILL").ToList(),
                BalanceItems = lstBalance.ToList(),
                TotalFeePartner = "0",
                TotalFeePartnerChu = "0",
                TotalRowsPayBill = 0,
                TotalRowsPinCode = 0,
                TotalRowsTopup = 0,
                IsAccountApi = user.AgentType == CommonConst.AgentType.AgentApi,
                FullName = user.FullName,
            };
            partnerInput.SumPinCodes = new ReportComparePartnerDto()
            {
                Fee = partnerInput.PinCodeItems.Sum(c => c.Fee),
                Value = partnerInput.PinCodeItems.Sum(c => c.Value),
                Discount = partnerInput.PinCodeItems.Sum(c => c.Discount),
                Price = partnerInput.PinCodeItems.Sum(c => c.Price),
                Quantity = partnerInput.PinCodeItems.Sum(c => c.Quantity),
            };

            partnerInput.SumPinGames = new ReportComparePartnerDto()
            {
                Fee = partnerInput.PinGameItems.Sum(c => c.Fee),
                Value = partnerInput.PinGameItems.Sum(c => c.Value),
                Discount = partnerInput.PinGameItems.Sum(c => c.Discount),
                Price = partnerInput.PinGameItems.Sum(c => c.Price),
                Quantity = partnerInput.PinGameItems.Sum(c => c.Quantity),
            };

            partnerInput.SumTopup = new ReportComparePartnerDto()
            {
                Fee = partnerInput.TopupItems.Sum(c => c.Fee),
                Value = partnerInput.TopupItems.Sum(c => c.Value),
                Discount = partnerInput.TopupItems.Sum(c => c.Discount),
                Price = partnerInput.TopupItems.Sum(c => c.Price),
                Quantity = partnerInput.TopupItems.Sum(c => c.Quantity),
            };

            partnerInput.SumTopupPostpaId = new ReportComparePartnerDto()
            {
                Fee = partnerInput.TopupPostpaIdItems.Sum(c => c.Fee),
                Value = partnerInput.TopupPostpaIdItems.Sum(c => c.Value),
                Discount = partnerInput.TopupPostpaIdItems.Sum(c => c.Discount),
                Price = partnerInput.TopupPostpaIdItems.Sum(c => c.Price),
                Quantity = partnerInput.TopupPostpaIdItems.Sum(c => c.Quantity),
            };

            partnerInput.SumTopupPrepaId = new ReportComparePartnerDto()
            {
                Fee = partnerInput.TopupPrepaIdItems.Sum(c => c.Fee),
                Value = partnerInput.TopupPrepaIdItems.Sum(c => c.Value),
                Discount = partnerInput.TopupPrepaIdItems.Sum(c => c.Discount),
                Price = partnerInput.TopupPrepaIdItems.Sum(c => c.Price),
                Quantity = partnerInput.TopupPrepaIdItems.Sum(c => c.Quantity),
            };

            partnerInput.SumData = new ReportComparePartnerDto()
            {
                Fee = partnerInput.DataItems.Sum(c => c.Fee),
                Value = partnerInput.DataItems.Sum(c => c.Value),
                Discount = partnerInput.DataItems.Sum(c => c.Discount),
                Price = partnerInput.DataItems.Sum(c => c.Price),
                Quantity = partnerInput.DataItems.Sum(c => c.Quantity),
            };

            partnerInput.SumPayBill = new ReportComparePartnerDto()
            {
                Fee = partnerInput.PayBillItems.Sum(c => c.Fee),
                Value = partnerInput.PayBillItems.Sum(c => c.Value),
                Discount = partnerInput.PayBillItems.Sum(c => c.Discount),
                Price = partnerInput.PayBillItems.Sum(c => c.Price),
                Quantity = partnerInput.PayBillItems.Sum(c => c.Quantity),
            };

            partnerInput.SumPayBill.DiscountVat = Math.Round(partnerInput.SumPayBill.Discount / 11, 0);
            partnerInput.SumPayBill.DiscountNoVat = partnerInput.SumPayBill.Discount - partnerInput.SumPayBill.DiscountVat;
            partnerInput.TotalRowsTopup = partnerInput.TopupItems.Count();
            partnerInput.TotalRowsPinCode = partnerInput.PinCodeItems.Count();
            partnerInput.TotalRowsPayBill = partnerInput.PayBillItems.Count();
            partnerInput.TotalRowsBalance = partnerInput.BalanceItems.Count();

            partnerInput.TotalRowsTopupPostpaId = partnerInput.TopupPostpaIdItems.Count();
            partnerInput.TotalRowsTopupPrepaId = partnerInput.TopupPrepaIdItems.Count();  
            partnerInput.TotalRowsPinGame = partnerInput.PinGameItems.Count();
            partnerInput.TotalRowsData = partnerInput.DataItems.Count();        
            return _excelExporter.ReportCompareParnerExportToFile(partnerInput);
        }

        public async Task<ReportAgentBalanceDto> GetReportAgentBalanceSum(
          GetReportAccountBalanceInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportAgentBalanceRequest>();
                request.Offset = 0;
                request.Limit = 1;

                var total = new ReportAgentBalanceDto();


                var rs = await _reportsManager.ReportAgentBalanceReport(request);
                if (rs.SumData != null)
                {
                    var sumList = rs.SumData.ConvertTo<List<ReportAgentBalanceDto>>();
                    var fBalance = sumList.FirstOrDefault();
                    total = new ReportAgentBalanceDto()
                    {
                        AgentCode = input.AgentCode,
                        BeforeAmount = fBalance.BeforeAmount,
                        InputAmount = fBalance.InputAmount,
                        AfterAmount = fBalance.AfterAmount,
                        SaleAmount = 0,
                    };
                }


                var saleList = await GetReportComparePartnerList(new GetReportComparePartnerInput()
                {
                    AgentCode = input.AgentCode,
                    FromDate = input.FromDate,
                    ToDate = input.ToDate,
                    SkipCount = 0,
                    MaxResultCount = 1,
                    Type = "EXPORT",
                });
                if (saleList.TotalData != null)
                {
                    var fSale = saleList.TotalData.ConvertTo<ReportComparePartnerDto>();
                    total.SaleAmount = fSale.Price;
                }
                return total;
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportAgentBalanceList error: {e}");
                return new ReportAgentBalanceDto();
            }
        }
    }
}
