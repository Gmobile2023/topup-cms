using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.PayBacks.Dtos;
using HLS.Topup.StockManagement.Importing;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;

namespace HLS.Topup.PayBacks.Importer
{
    public class PayBacksListExcelDataReader : NpoiExcelImporterBase<PayBacksImportDto>, IPayBacksListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;
        private readonly ILogger<PayBacksListExcelDataReader> _logger;
        public PayBacksListExcelDataReader(ILocalizationManager localizationManager, ILogger<PayBacksListExcelDataReader> logger)
        {
            _logger = logger;
            _localizationSource = localizationManager.GetSource(TopupConsts.LocalizationSourceName);
        }
        
        public List<PayBacksImportDto> GetPayBacksFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }
        
        private PayBacksImportDto ProcessExcelRow(ISheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var payBacks = new PayBacksImportDto();

            try
            {
                payBacks.AgentCode = GetRequiredValueFromRowOrNull(worksheet, row, 0, nameof(payBacks.AgentCode), exceptionMessage);
                payBacks.Amount =  worksheet.GetRow(row).Cells[1].NumericCellValue;
            }
            catch (System.Exception exception)
            {
                _logger.LogError($"ProcessExcelRow error: {exception}");
            }

            return payBacks;
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