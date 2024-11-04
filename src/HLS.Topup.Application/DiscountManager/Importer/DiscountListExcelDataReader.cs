using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.DiscountManager.Dtos;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace HLS.Topup.DiscountManager.Importer
{
    public class DiscountListExcelDataReader : NpoiExcelImporterBase<DiscountImportDto>, IDiscountListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;
        private readonly ILogger<DiscountListExcelDataReader> _logger;

        public DiscountListExcelDataReader(ILocalizationManager localizationManager,
            ILogger<DiscountListExcelDataReader> logger)
        {
            _logger = logger;
            _localizationSource = localizationManager.GetSource(TopupConsts.LocalizationSourceName);
        }

        public List<DiscountImportDto> GetDiscountFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private DiscountImportDto ProcessExcelRow(ISheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var discount = new DiscountImportDto();

            try
            {
                // CellReference ar = new CellReference("A"+row+1);
                // CellReference br = new CellReference("B"+row+1);
                // CellReference cr = new CellReference("C"+row+1);
                // var acell = worksheet.GetRow(ar.Row).GetCell(ar.Col);
                // var bcell = worksheet.GetRow(br.Row).GetCell(br.Col);
                // var ccell = worksheet.GetRow(cr.Row).GetCell(cr.Col);
                
                discount.ProductCode = GetRequiredValueFromRowOrNull(worksheet, row, 0, nameof(discount.ProductCode), exceptionMessage);
                discount.DiscountValue = Decimal.Parse(worksheet.GetRow(row).Cells[1].ToString().Trim()) != null ? Decimal.Parse(worksheet.GetRow(row).Cells[1].ToString().Trim()) : (decimal?)null;
                discount.FixAmount = Decimal.Parse(worksheet.GetRow(row).Cells[2].ToString().Trim()) != null ? Decimal.Parse(worksheet.GetRow(row).Cells[2].ToString().Trim()) : (decimal?)null;
            }
            catch (System.Exception exception)
            {
                _logger.LogError($"ProcessExcelRow error: {exception}");
            }

            return discount;
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