using System.Collections.Generic;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Dto;

namespace HLS.Topup.StockManagement.Exporting
{
    public interface ICardStocksExcelExporter
    {
        FileDto ExportToFile(List<CardStockDto> cardStocks);
    }
}