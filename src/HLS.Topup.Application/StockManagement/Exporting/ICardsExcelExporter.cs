using System.Collections.Generic;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Stock;

namespace HLS.Topup.StockManagement.Exporting
{
    public interface ICardsExcelExporter
    {
        FileDto ExportToFile(List<CardResponseDto> cards);

        FileDto ExportListToFile(List<StockTransRequestDto> cards);
    }
}