using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Compare;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.Report;
using HLS.Topup.Reports.Dtos;
using static HLS.Topup.Report.ReportComparePartnerExportInfo;

namespace HLS.Topup.Reports
{
    public interface IReportsManager
    {
        Task<ApiResponseDto<List<ReportCardStockHistoriesDto>>> ReportCardStockHistories(ReportCardStockHistoriesRequest request);
        Task<ApiResponseDto<List<ReportCardStockInventoryDto>>> ReportCardStockInventory(ReportCardStockInventoryRequest request);

        Task<ApiResponseDto<List<ReportCardStockImExPortDto>>> ReportCardStockImExPort(ReportCardStockImExPortRequest request);
        Task<ApiResponseDto<List<ReportCardStockImExPortDto>>> ReportCardStockImExProvider(ReportCardStockImExProviderRequest request);
        Task<ApiResponseDto<List<ReportDetailDto>>> ReportDetailGetRequest(ReportDetailRequest request);
        Task<ApiResponseDto<List<ReportBalanceTotalDto>>> BalanceTotalGetRequest(BalanceTotalRequest request);
        Task<ApiResponseDto<List<ReportBalanceTotalDto>>> BalanceGroupTotalGetRequest(BalanceGroupTotalRequest request);
        Task<ApiResponseDto<List<SimBalanceHistoriesDto>>> ReportSimTransactionDetailRequest(SimBalanceHistoriesRequest request);
        Task<ApiResponseDto<List<SimBalanceByDateDto>>> ReportSimTransactionByDateRequest(SimBalanceDateRequest request);
        Task<ApiResponseDto<List<ReportTransDetailDto>>> ReportTransDetailReport(ReporttransDetailRequest request);
        Task<ReportItemDetailDto> ReportTransDetailQuery(TransDetailByTransCodeRequest request);
        Task<ApiResponseDto<List<ReportDebtDetailDto>>> ReportDebtDetailReport(ReportDebtDetailRequest request);
        Task<ApiResponseDto<List<ReportItemTotalDebt>>> ReportTotalDebtReport(ReportTotalDebtRequest request);
        Task<ApiResponseDto<List<ReportItemTotalDay>>> ReportTotalDayReport(ReportTotalDayRequest request);
        Task<ApiResponseDto<List<ReportRefundDetailDto>>> ReportRefundDetailReport(ReportRefundDetailRequest request);

        Task<ApiResponseDto<List<ReportTransferDetailDto>>> ReportTransferDetailReport(ReportTransferDetailRequest request);

        Task<ApiResponseDto<List<ReportServiceDetailDto>>> ReportServiceDetailReport(ReportServiceDetailRequest request);

        Task<ApiResponseDto<List<ReportServiceTotalDto>>> ReportServiceTotalReport(ReportServiceTotalRequest request);

        Task<ApiResponseDto<List<ReportServiceProviderDto>>> ReportServiceProviderReport(ReportServiceProviderRequest request);

        Task<ApiResponseDto<List<ReportAgentBalanceDto>>> ReportAgentBalanceReport(ReportAgentBalanceRequest request);

        Task<ApiResponseDto<List<ReportRevenueAgentDto>>> ReportRevenueAgentReport(ReportRevenueAgentRequest request);

        Task<ApiResponseDto<List<ReportRevenueCityDto>>> ReportRevenueCityReport(ReportRevenueCityRequest request);

        Task<ApiResponseDto<List<ReportTotalSaleAgentDto>>> ReportTotalSaleAgentReport(ReportTotalSaleAgentRequest request);

        Task<ApiResponseDto<List<ReportRevenueActiveDto>>> ReportRevenueActiveReport(ReportRevenueActiveRequest request);

        Task<GetRevenueInDayDto> GetRevenueInDayRequest(GetRevenueInDayRequest input);

        Task<ApiResponseDto<List<CompareDtoReponse>>> ReportCompareList(ReportCompareListRequest request);

        Task<ApiResponseDto<List<CompareRefunDto>>> ReportCompareRefundList(ReportCompareRefundRequest request);

        Task<CompareRefunDto> ReportCompareRefundSingle(ReportCompareRefundSingleRequest request);

        Task<ResponseMessages> RefundCompareProvinder(CompareRefundCompareRequest request);

        Task<ResponseMessages> CompareProviderDate(CompareProviderRequest request);

        Task<ApiResponseDto<List<CompareRefunDetailDto>>> ReportCompareRefundDetailList(
       ReportCompareRefundDetailRequest request);

        Task<ApiResponseDto<List<CompareReponseDto>>> ReportCompareReonseList(
        ReportCompareReonseRequest request);

        Task<ApiResponseDto<List<CompareRefunDetailDto>>> ReportCompareDetailReonseList(
        ReportCompareDetailReonseRequest request);

        Task<ApiResponseDto<List<ReportRevenueDashboardDay>>> ReportRevenueDashboardDayList(
         ReportRevenueDashBoardDayRequest request);

        Task<ResponseMessages> CheckProviderCompareDate(ReportCheckCompareRequest request);

        Task<ApiResponseDto<List<ReportCardStockAutoDto>>> ReportCardStockAuto(ReportCardStockAutoRequest request);
        Task ReportSyncAccountReport(SyncAccountRequest request);

        Task<ApiResponseDto<List<ReportComparePartnerDto>>> ReportComparePartner(
       ReportComparePartnerRequest request);


        Task<ApiResponseDto<List<ReportBalancePartnerDto>>> ReportBalancePartner(
        ReportComparePartnerRequest request);


        Task<bool> SendmailReportComparePartner(SendMailComparePartnerRequest request);

        Task InsertSmsMessage(SmsMessageRequest request);

        Task<ApiResponseDto<List<ReportCommissionDetailDto>>> ReportCommissionDetailReport(ReportCommissionDetailRequest request);

        Task<ApiResponseDto<List<ReportCommissionTotalDto>>> ReportCommissionTotalReport(ReportCommissionTotalRequest request);

        Task<ApiResponseDto<List<ReportCommissionAgentDetailDto>>> ReportCommissionAgentDetailReport(ReportCommissionAgentDetailRequest request);

        Task<ApiResponseDto<List<ReportCommissionAgentTotalDto>>> ReportCommissionAgentTotalReport(ReportCommissionAgentTotalRequest request);

        Task<ApiResponseDto<List<ReportRevenueCommistionDashDay>>> ReportAgentGeneralDashDayList(ReportAgentGeneralDashRequest request);

        Task<ApiResponseDto<List<ReportTopupRequestLogDto>>> ReportTopupRequestLogList(ReportTopupRequestLogRequest request);
    }
}
