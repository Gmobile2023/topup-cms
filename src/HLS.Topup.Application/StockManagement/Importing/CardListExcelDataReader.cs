using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.StockManagement.Dtos;
using Microsoft.Extensions.Logging;
using NLog;
using NPOI.SS.UserModel;
using TW.CardMapping.Authorization.Users.Importing.Dto;

namespace HLS.Topup.StockManagement.Importing
{
    public class CardListExcelDataReader : NpoiExcelImporterBase<CardImportItem>, ICardListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;
        //private readonly Logger _logger = LogManager.GetLogger("CardListExcelDataReader");
        private readonly ILogger<CardListExcelDataReader> _logger;
        public CardListExcelDataReader(ILocalizationManager localizationManager, ILogger<CardListExcelDataReader> logger)
        {
            _logger = logger;
            _localizationSource = localizationManager.GetSource(TopupConsts.LocalizationSourceName);
        }

        public List<CardImportItem> GetCardsFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private CardImportItem ProcessExcelRow(ISheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }
            if (worksheet.SheetName == "Category" || worksheet.SheetName == "Service")
            {
                return null;
            }
            var exceptionMessage = new StringBuilder();
            var card = new CardImportItem();

            try
            {
                card.ServiceCode = GetRequiredValueFromRowOrNull(worksheet, row, 0, nameof(card.ServiceCode), exceptionMessage);
                card.CategoryCode = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(card.CategoryCode), exceptionMessage);
                card.Serial = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(card.Serial), exceptionMessage);
                card.CardCode = GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(card.CardCode), exceptionMessage);  
                card.CardValue = Int32.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(card.CardValue), exceptionMessage));
                if(worksheet.GetRow(row).Cells[5].CellType == CellType.String){
                    DateTime _eDate = DateTime.ParseExact(GetRequiredValueFromRowOrNull(worksheet, row, 5, "", exceptionMessage), "dd/MM/yyyy", null);
                    card.ExpiredDate = _eDate;
                }else if (worksheet.GetRow(row).Cells[5].CellType == CellType.Numeric)
                {
                    card.ExpiredDate = worksheet.GetRow(row).Cells[5].DateCellValue;
                } 
            }
            catch (System.Exception exception)
            {
                _logger.LogError($"ProcessExcelRow error: {exception}");
                card.Exception = exception.Message;
            }

            return card;
        }

        private string GetRequiredValueFromRowOrNull(ISheet worksheet, int row, int column, string columnName,
            StringBuilder exceptionMessage)
        {
            var cellValue = worksheet.GetRow(row).Cells[column].StringCellValue;
            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue))
            {
                return cellValue;
            }

            exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName));
            return null;
        }

        private string[] GetAssignedRoleNamesFromRow(ISheet worksheet, int row, int column)
        {
            var cellValue = worksheet.GetRow(row).Cells[column].StringCellValue;
            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue))
            {
                return new string[0];
            }

            return cellValue.ToString().Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim())
                .ToArray();
        }

        private string GetLocalizedExceptionMessagePart(string parameter)
        {
            return _localizationSource.GetString("{0}IsInvalid", _localizationSource.GetString(parameter)) + "; ";
        }

        private bool IsRowEmpty(ISheet worksheet, int row)
        {
            var cell = worksheet.GetRow(row)?.Cells.FirstOrDefault();
            return cell == null || string.IsNullOrWhiteSpace(cell.StringCellValue);
        }
    }
}
