using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Transactions;
using HLS.Topup.Report;
using HLS.Topup.Reports.Dtos;

namespace HLS.Topup.Reports
{
    public interface IReportSystemAppService
    {
        Task<bool> SendMailReportComparePartner(SendMailComparePartnerRequest request);

        Task<PagedResultDtoReport<ReportBalancePartnerDto>> GetReportBalancePartner(GetReportBalancePartnerInput input);
        Task<PagedResultDtoReport<ReportDetailDto>> GetReportDetailList(GetReportDetailInput input);

        Task<PagedResultDtoReport<ReportTotalDto>> GetReportTotalList(GetReportTotalInput input);

        Task<PagedResultDtoReport<ReportGroupDto>> GetReportGroupList(GetReportGroupInput input);

        Task<PagedResultDtoReport<ReportTransDetailDto>> GetReportTransDetailList(GetReportTransDetailInput input);

        Task<TransactionResponseDtoApp> GetReportTransInfoRequest(TransRequestByTransCodeInput input);

        Task<PagedResultDtoReport<ReportItemTotalDay>> GetReportTotalDayList(GetReportTotalDayInput input);

        Task<FileDto> GetReportDetailListToExcel(GetAllReportDetailForExcelInput input);

        Task<FileDto> GetReportTotalListToExcel(GetAllReportTotalForExcelInput input);

        Task<FileDto> GetReportGroupListToExcel(GetAllReportGroupForExcelInput input);


        Task<PagedResultDtoReport<ReportCardStockHistoriesDto>> GetReportCardStockHistories(
            GetReportCardStockHistoriesInput input);

        Task<PagedResultDtoReport<ReportCardStockInventoryDto>> GetReportCardStockInventory(
            GetReportCardStockInventoryInput input);

        Task<FileDto> GetReportCardStockHistoriesToExcel(GetReportCardStockHistoriesInput input);
        Task<FileDto> GetReportCardStockInventoryToExcel(GetReportCardStockInventoryInput input);

        Task<FileDto> GetReportTransDetailListToExcel(GetAllReportTransDetailForExcelInput input);

        Task<FileDto> GetReportTotalDayListToExcel(GetAllReportTotalDayForExcelInput input);

        Task<PagedResultDtoReport<ReportDebtDetailDto>> GetReportDebtDetailList(GetReportdebtDetailInput input);

        Task<PagedResultDtoReport<ReportItemTotalDebt>> GetReportTotalDebtList(GetReportTotalDebtInput input);

        Task<FileDto> GetReportTotalDebtListToExcel(GetReportTotalDebtInput input);

        Task<FileDto> GetReportDebtDetailListToExcel(GetReportdebtDetailInput input);

        Task<PagedResultDtoReport<ReportTransferDetailDto>> GetReportTransferDetailList(GetReportTransferDetailInput input);

        Task<PagedResultDtoReport<ReportServiceDetailDto>> GetReportServiceDetailList(GetReportServiceDetailInput input);

        Task<PagedResultDtoReport<ReportServiceTotalDto>> GetReportServiceTotalList(GetReportServiceTotalInput input);

        Task<PagedResultDtoReport<ReportServiceProviderDto>> GetReportServiceProviderList(GetReportServiceProviderInput input);

        Task<PagedResultDtoReport<ReportAgentBalanceDto>> GetReportAgentBalanceList(GetReportAgentBalanceInput input);

        Task<FileDto> GetReportRefundDetailListToExcel(GetReportRefundDetailInput input);

        Task<FileDto> GetReportTransferDetailListToExcel(GetReportTransferDetailInput input);

        Task<FileDto> GetReportServiceDetailListToExcel(GetReportServiceDetailInput input);

        Task<FileDto> GetReportServiceTotalListToExcel(GetReportServiceTotalInput input);

        Task<FileDto> GetReportServiceProviderListToExcel(GetReportServiceProviderInput input);

        Task<FileDto> GetReportAgentBalanceListToExcel(GetReportAgentBalanceInput input);

        Task<PagedResultDtoReport<ReportRevenueAgentDto>> GetReportRevenueAgentList(GetReportRevenueAgentInput input);

        Task<PagedResultDtoReport<ReportRevenueCityDto>> GetReportRevenueCityList(GetReportRevenueCityInput input);

        Task<PagedResultDtoReport<ReportTotalSaleAgentDto>> GetReportTotalSaleAgentList(GetReportTotalSaleAgentInput input);

        Task<PagedResultDtoReport<ReportRevenueActiveDto>> GetReportRevenueActiveList(GetReportRevenueActiveInput input);

        Task<PagedResultDtoReport<ReportRevenueCommistionDashDay>> GetReportAgentGeneralCommistionDashList(
          GetDashAgentGeneralboardInput input);

        Task<PagedResultDtoReport<ReportRevenueDashboardDay>> GetReportRevenueDashboardList(GetReportRevenueDashboardInput input);

        Task<List<DashRevenueItem>> GetDashboardListRevenue(GetDashboardRevenueInput input);

        Task<FileDto> GetReportRevenueAgentListToExcel(GetReportRevenueAgentInput input);

        Task<FileDto> GetReportRevenueCityListToExcel(GetReportRevenueCityInput input);

        Task<FileDto> GetReportTotalSaleAgentListToExcel(GetReportTotalSaleAgentInput input);

        Task<FileDto> GetReportRevenueActiveListToExcel(GetReportRevenueActiveInput input);
        Task<GetRevenueInDayDto> GetRevenueInDayRequest(GetRevenueInDayInput input);

        Task<PagedResultDtoReport<ReportCardStockImExPortDto>> GetReportCardStockImExPort(
           GetReportCardStockImExPortInput input);

        Task<PagedResultDtoReport<ReportCardStockImExPortDto>> GetReportCardStockImExProvider(
           GetReportCardStockImExProviderInput input);

        Task<FileDto> GetReportCardStockImExProviderToExcel(GetReportCardStockImExProviderInput input);

        Task<FileDto> GetReportRevenueDashboardListToExcel(GetReportRevenueDashboardInput input);

        Task<FileDto> GetReportAgentGeneralCommistionListToExcel(GetDashAgentGeneralboardInput input);

        Task<FileDto> GetReportComparePartnerToExcel(GetReportComparePartnerInput input);

        Task<ReportAgentBalanceDto> GetReportAgentBalanceSum(GetReportAccountBalanceInput input);

        Task<DashAgentGenerals> GetDashAgentGeneralCommistion(GetDashAgentGeneralInput input);

        Task<FileDto> GetReportTopupRequestLogListToExcel(GetReportTopupRequestDetailInput input);
    }
}
