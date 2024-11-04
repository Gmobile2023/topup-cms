using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Hangfire;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Authorization.Users.Dto;
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
using NLog;
using ServiceStack;


namespace HLS.Topup.Reports
{ 
    public partial class ReportSystemAppService
    {
        public async Task<PagedResultDtoReport<ReportCommissionDetailDto>> GetReportCommissionDetailList(
          GetReportCommissionDetailInput input)
        {
            try
            {
               var request = input.ConvertTo<ReportCommissionDetailRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                var user = await _userRepository.GetAsync(AbpSession.UserId ?? 0);              
                request.LoginCode = user.AccountCode;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Detail",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportCommissionDetailDto>(0,
                  new ReportCommissionDetailDto(),
                  new List<ReportCommissionDetailDto>(), warning: msg);

                var rs = await _reportsManager.ReportCommissionDetailReport(request);
                var totalCount = rs.Total;
                var sumList = rs.SumData.ConvertTo<List<ReportCommissionDetailDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportCommissionDetailDto();
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportCommissionDetailDto>(0, new ReportCommissionDetailDto(),
                        new List<ReportCommissionDetailDto>());

                var lst = rs.Payload.ConvertTo<List<ReportCommissionDetailDto>>();

                return new PagedResultDtoReport<ReportCommissionDetailDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportCommissionDetailList error: {e}");
                return new PagedResultDtoReport<ReportCommissionDetailDto>(0,
                    new ReportCommissionDetailDto(),
                    new List<ReportCommissionDetailDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportCommissionTotalDto>> GetReportCommissionTotalList(
            GetReportCommissionTotalInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportCommissionTotalRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                var user = await _userRepository.GetAsync(AbpSession.UserId ?? 0);                
                request.LoginCode = user.AccountCode;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                  return new PagedResultDtoReport<ReportCommissionTotalDto>(0,
                  new ReportCommissionTotalDto(),
                  new List<ReportCommissionTotalDto>(), warning: msg);

                var rs = await _reportsManager.ReportCommissionTotalReport(request);
                var totalCount = rs.Total;

                var sumList = rs.SumData.ConvertTo<List<ReportCommissionTotalDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportCommissionTotalDto();

                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportCommissionTotalDto>(0, new ReportCommissionTotalDto(),
                        new List<ReportCommissionTotalDto>());

                var lst = rs.Payload.ConvertTo<List<ReportCommissionTotalDto>>();
                return new PagedResultDtoReport<ReportCommissionTotalDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportCommissionTotalList error: {e}");
                return new PagedResultDtoReport<ReportCommissionTotalDto>(
                    0,
                    new ReportCommissionTotalDto(),
                    new List<ReportCommissionTotalDto>());
            }
        }

        public async Task<FileDto> GetReportCommissionDetailListToExcel(GetReportCommissionDetailInput input)
        {
            var request = input.ConvertTo<ReportCommissionDetailRequest>();
            request.Offset = 0;
            request.Limit = int.MaxValue;
            var user = await _userRepository.GetAsync(AbpSession.UserId ?? 0);            
            request.LoginCode = user.AccountCode;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Detail",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportCommissionDetailReport(request);
            var lst = rs.Payload.ConvertTo<List<ReportCommissionDetailDto>>();
            return _excelExporter.ReportCommissionDetailExportToFile(lst);
        }

        public async Task<FileDto> GetReportCommissionTotalListToExcel(GetReportCommissionTotalInput input)
        {
            var request = input.ConvertTo<ReportCommissionTotalRequest>();
            request.Offset = 0;
            request.Limit = int.MaxValue;
            var user = await _userRepository.GetAsync(AbpSession.UserId ?? 0);            
            request.LoginCode = user.AccountCode;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportCommissionTotalReport(request);
            var lst = rs.Payload.ConvertTo<List<ReportCommissionTotalDto>>();
            return _excelExporter.ReportCommissionTotalExportToFile(lst);
        }


        public async Task<PagedResultDtoReport<ReportCommissionAgentDetailDto>> GetReportCommissionAgentDetailList(
         GetReportCommissionAgentDetailInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportCommissionAgentDetailRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                var user = await _userRepository.GetAsync(AbpSession.UserId ?? 0);
                request.LoginCode = user.AccountCode;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Detail",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportCommissionAgentDetailDto>(0,
                    new ReportCommissionAgentDetailDto(),
                    new List<ReportCommissionAgentDetailDto>(), warning: msg);

                var rs = await _reportsManager.ReportCommissionAgentDetailReport(request);
                var totalCount = rs.Total;
                var sumList = rs.SumData.ConvertTo<List<ReportCommissionAgentDetailDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportCommissionAgentDetailDto();
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportCommissionAgentDetailDto>(0, new ReportCommissionAgentDetailDto(),
                        new List<ReportCommissionAgentDetailDto>());

                var lst = rs.Payload.ConvertTo<List<ReportCommissionAgentDetailDto>>();

                return new PagedResultDtoReport<ReportCommissionAgentDetailDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportCommissionAgentDetailList error: {e}");
                return new PagedResultDtoReport<ReportCommissionAgentDetailDto>(
                    0,
                    new ReportCommissionAgentDetailDto(),
                    new List<ReportCommissionAgentDetailDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportCommissionAgentTotalDto>> GetReportCommissionAgentTotalList(
            GetReportCommissionAgentTotalInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportCommissionAgentTotalRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                var user = await _userRepository.GetAsync(AbpSession.UserId ?? 0);               
                request.LoginCode = user.AccountCode;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportCommissionAgentTotalDto>(0,
                    new ReportCommissionAgentTotalDto(),
                    new List<ReportCommissionAgentTotalDto>(), warning: msg);

                var rs = await _reportsManager.ReportCommissionAgentTotalReport(request);
                var totalCount = rs.Total;

                var sumList = rs.SumData.ConvertTo<List<ReportCommissionAgentTotalDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportCommissionAgentTotalDto();

                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportCommissionAgentTotalDto>(0, new ReportCommissionAgentTotalDto(),
                        new List<ReportCommissionAgentTotalDto>());

                var lst = rs.Payload.ConvertTo<List<ReportCommissionAgentTotalDto>>();
                return new PagedResultDtoReport<ReportCommissionAgentTotalDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportCommissionAgentTotalList error: {e}");
                return new PagedResultDtoReport<ReportCommissionAgentTotalDto>(
                    0,
                    new ReportCommissionAgentTotalDto(),
                    new List<ReportCommissionAgentTotalDto>());
            }
        }

        public async Task<FileDto> GetReportCommissionAgentDetailListToExcel(GetReportCommissionAgentDetailInput input)
        {
            var request = input.ConvertTo<ReportCommissionAgentDetailRequest>();
            request.Offset = 0;
            request.Limit = int.MaxValue;
            var user = await _userRepository.GetAsync(AbpSession.UserId ?? 0);
            request.LoginCode = user.AccountCode;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportCommissionAgentDetailReport(request);
            var lst = rs.Payload.ConvertTo<List<ReportCommissionAgentDetailDto>>();
            return _excelExporter.ReportCommissionAgentDetailExportToFile(lst);
        }

        public async Task<FileDto> GetReportCommissionAgentTotalListToExcel(GetReportCommissionAgentTotalInput input)
        {
            var request = input.ConvertTo<ReportCommissionAgentTotalRequest>();
            request.Offset = 0;
            request.Limit = int.MaxValue;
            var user = await _userRepository.GetAsync(AbpSession.UserId ?? 0);
            request.LoginCode = user.AccountCode;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportCommissionAgentTotalReport(request);
            var lst = rs.Payload.ConvertTo<List<ReportCommissionAgentTotalDto>>();
            return _excelExporter.ReportCommissionAgentTotalExportToFile(lst);
        }
       
    }
}
