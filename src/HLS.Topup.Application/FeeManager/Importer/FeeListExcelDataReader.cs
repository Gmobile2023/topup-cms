using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.FeeManager.Dtos;
using HLS.Topup.FeeManager.Importer;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;

namespace HLS.Topup.FeeManager.Importer
{
    public class FeeListExcelDataReader : NpoiExcelImporterBase<FeeImportDto>, IFeeListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;
        private readonly ILogger<FeeListExcelDataReader> _logger;

        public FeeListExcelDataReader(ILocalizationManager localizationManager,
            ILogger<FeeListExcelDataReader> logger)
        {
            _logger = logger;
            _localizationSource = localizationManager.GetSource(TopupConsts.LocalizationSourceName);
        }

        public List<FeeImportDto> GetFeeImportFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private FeeImportDto ProcessExcelRow(ISheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var fee = new FeeImportDto();

            try
            {
                fee.ProductCode = GetRequiredValueFromRowOrNull(worksheet, row, 0, nameof(fee.ProductCode), exceptionMessage);
                fee.MinFee = (worksheet.GetRow(row).Cells[1].ToString().Trim() != null) ? Decimal.Parse(worksheet.GetRow(row).Cells[1].ToString().Trim()) : (decimal?)null;
                fee.AmountMinFee = (worksheet.GetRow(row).Cells[2].ToString().Trim() != null) ? Decimal.Parse(worksheet.GetRow(row).Cells[2].ToString().Trim()) : (decimal?)null;
                fee.AmountIncrease = (worksheet.GetRow(row).Cells[3].ToString().Trim() != null) ? Decimal.Parse(worksheet.GetRow(row).Cells[3].ToString().Trim()) : (decimal?)null;
                fee.SubFee = (worksheet.GetRow(row).Cells[4].ToString().Trim() != null) ? Decimal.Parse(worksheet.GetRow(row).Cells[4].ToString().Trim()) : (decimal?)null;
            }
            catch (System.Exception exception)
            {
                _logger.LogError($"ProcessExcelRow error: {exception}");
            }

            return fee;
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