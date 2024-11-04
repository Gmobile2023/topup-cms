using System.Collections.Generic;
using HLS.Topup.Dto;
using HLS.Topup.Report;
using HLS.Topup.Reports.Dtos;
using static HLS.Topup.Report.ReportComparePartnerExportInfo;

namespace HLS.Topup.Reports.Exporting
{
    public interface IReportExcelExporter
    {
        FileDto ReportDetailExportToFile(List<ReportDetailDto> input);

        FileDto ReportTotalExportToFile(List<ReportTotalDto> input);

        FileDto ReportGroupExportToFile(List<ReportGroupDto> input);
        FileDto ReportCardStockHistoriesToFile(List<ReportCardStockHistoriesDto> input);
        FileDto ReportCardStockInventoryToFile(List<ReportCardStockInventoryDto> input);

        FileDto ExportAutoCampareStockToFile(List<ReportCardStockInventoryDto> cardStocks);

        FileDto ReportTransDetailExportToFile(List<ReportTransDetailDto> input);

        FileDto ReportTotalDayExportToFile(List<ReportItemTotalDay> input);

        FileDto ReportDebtDetailExportToFile(List<ReportDebtDetailDto> input);

        FileDto ReportTotalDebtExportToFile(List<ReportItemTotalDebt> input);

        FileDto ReportRefundDetailExportToFile(List<ReportRefundDetailDto> input);

        FileDto ReportTransferDetailExportToFile(List<ReportTransferDetailDto> input);

        FileDto ReportServiceDetailExportToFile(List<ReportServiceDetailDto> input);

        FileDto ReportServiceTotalExportToFile(List<ReportServiceTotalDto> input);

        FileDto ReportServiceProviderExportToFile(List<ReportServiceProviderDto> input);

        FileDto ReportAgentBalanceExportToFile(List<ReportAgentBalanceDto> input);

        FileDto ReportRevenueAgentExportToFile(List<ReportRevenueAgentDto> input);

        FileDto ReportRevenueCityExportToFile(List<ReportRevenueCityDto> input);

        FileDto ReportTotalSaleAgentExportToFile(List<ReportTotalSaleAgentDto> input);

        FileDto ReportRevenueActiveExportToFile(List<ReportRevenueActiveDto> input);

        FileDto ReportCardStockImExPortToFile(List<ReportCardStockImExPortDto> input);

        FileDto ReportCardStockAutoToFile(List<ReportCardStockAutoDto> input, string date);

        FileDto ExportRevenueDashboardListExportToFile(List<ReportRevenueDashboardDay> input);

        FileDto ExportAgentGeneralCommistionDashListExportToFile(List<ReportRevenueCommistionDashDay> input);

        FileDto ReportCompareParnerExportToFile(ReportComparePartnerExportInfo input);

        FileDto ReportCommissionDetailExportToFile(List<ReportCommissionDetailDto> input);

        FileDto ReportCommissionTotalExportToFile(List<ReportCommissionTotalDto> input);

        FileDto ReportCommissionAgentDetailExportToFile(List<ReportCommissionAgentDetailDto> input);

        FileDto ReportCommissionAgentTotalExportToFile(List<ReportCommissionAgentTotalDto> input);

        FileDto ReportTopupRequestLogExportToFile(List<ReportTopupRequestLogDto> input);
    }
}