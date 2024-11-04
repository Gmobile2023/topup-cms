using System.Collections.Generic;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Transactions;

namespace HLS.Topup.Transactions.Exporting
{
    public interface ITransactionsExcelExporter
    {
        FileDto ExportToFile(List<TopupRequestResponseDto> topupRequests, string fileName);
        FileDto ExportItemsToFile(List<TopupDetailResponseDTO> topupRequests);
        FileDto ExportToFilePartner(List<TopupRequestResponseDto> topupRequests);
        FileDto TopupDetailExportToFile(List<TopupDetailResponseDTO> topupRequests);
        FileDto TopupDetailRequestExportToFile(List<TopupDetailResponseDTO> topupDetailResponse, string fileName);

        FileDto BatchLotExportToFile(List<BatchItemDto> items);

        FileDto BatchLotTopupDetailExportToFile(List<BatchDetailDto> items);

        FileDto BatchLotBillDetailExportToFile(List<BatchDetailDto> items);

        FileDto BatchLotPinCodeDetailExportToFile(List<BatchDetailDto> items);

        FileDto OffsetTopupExportToFile(List<SaleOffsetReponseDto> items);
    }
}