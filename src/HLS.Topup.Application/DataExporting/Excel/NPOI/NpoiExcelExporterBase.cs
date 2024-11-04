using System;
using System.Collections.Generic;
using System.IO;
using Abp.AspNetZeroCore.Net;
using Abp.Collections.Extensions;
using Abp.Dependency;
using HLS.Topup.Dto;
using HLS.Topup.Storage;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using ServiceStack;

namespace HLS.Topup.DataExporting.Excel.NPOI
{
    public abstract class NpoiExcelExporterBase : TopupServiceBase, ITransientDependency
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;

        protected NpoiExcelExporterBase(ITempFileCacheManager tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }

        protected FileDto CreateExcelPackage(string fileName, Action<XSSFWorkbook> creator)
        {
            var file = new FileDto(fileName,
                MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            var workbook = new XSSFWorkbook();

            creator(workbook);

            Save(workbook, file);

            return file;
        }

        protected void AddHeader(ISheet sheet, params string[] headerTexts)
        {
            if (headerTexts.IsNullOrEmpty())
            {
                return;
            }

            sheet.CreateRow(0);

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeader(sheet, i, headerTexts[i]);
            }
        }

        protected void AddHeader(ISheet sheet, int columnIndex, string headerText)
        {
            var cell = sheet.GetRow(0).CreateCell(columnIndex);
            cell.SetCellValue(headerText);
            var cellStyle = sheet.Workbook.CreateCellStyle();
            var font = sheet.Workbook.CreateFont();
            font.IsBold = true;
            font.FontHeightInPoints = 12;
            cellStyle.SetFont(font);
            cell.CellStyle = cellStyle;
        }

        protected void AddObjects_old<T>(ISheet sheet, int startRowIndex, IList<T> items,
            params Func<T, object>[] propertySelectors)
        {
            if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 1; i <= items.Count; i++)
            {
                var row = sheet.CreateRow(i);

                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    var cell = row.CreateCell(j);
                    var value = propertySelectors[j](items[i - 1]);
                    if (value != null)
                    {
                        cell.SetCellValue(value.ToString());
                    }
                }
            }
        }

        protected void Save(XSSFWorkbook excelPackage, FileDto file)
        {
            using (var stream = new MemoryStream())
            {
                excelPackage.Write(stream);
                _tempFileCacheManager.SetFile(file.FileToken, stream.ToArray());
            }
        }

        protected void SetCellDataFormat(ICell cell, string dataFormat)
        {
            if (cell == null)
            {
                return;
            }

            var dateStyle = cell.Sheet.Workbook.CreateCellStyle();
            var format = cell.Sheet.Workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat(dataFormat);
            cell.CellStyle = dateStyle;
            if (DateTime.TryParse(cell.StringCellValue, out var datetime))
            {
                cell.SetCellValue(datetime);
            }
        }

        protected void SetCellAmountFormat(ICell cell)
        {
            if (cell == null)
            {
                return;
            }

            cell.SetCellType(CellType.Numeric);
            if (double.TryParse(cell.StringCellValue, out var val))
            {
                cell.SetCellValue(val);
            }
        }

        protected void AddObjects<T>(ISheet sheet, int startRowIndex, IList<T> items,
            params Func<T, object>[] propertySelectors)
        {
            if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 1; i <= items.Count; i++)
            {
                var row = sheet.CreateRow(i);
                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    var cell = row.CreateCell(j);
                    var cellObj = propertySelectors[j](items[i - 1]);
                    CellOption cellData;
                    if (cellObj == null)
                    {
                        cellData = CellOption.Create("");
                    }
                    else if (cellObj.GetType() == typeof(CellOption))
                    {
                        cellData = cellObj.ConvertTo<CellOption>();
                    }
                    else
                    {
                        cellData = CellOption.Create(cellObj);
                    }

                    if (cellData != null && cellData.Value != null)
                    {
                        if (cellData.Format.ToLower().StartsWith("dd") ||
                            cellData.Format.ToLower().StartsWith("mm") ||
                            cellData.Format.ToLower().StartsWith("yyyy"))
                        {
                            var dateStyle = cell.Sheet.Workbook.CreateCellStyle();
                            dateStyle.DataFormat =
                                cell.Sheet.Workbook.CreateDataFormat().GetFormat(cellData.Format);
                            cell.CellStyle = dateStyle;
                            if (DateTime.TryParse(cellData.Value.ToString(), out var datetime))
                            {
                                cell.SetCellValue(datetime);
                            }
                        }
                        else if (cellData.Format.ToLower() == "number")
                        {
                            cell.SetCellType(CellType.Numeric);
                            if (double.TryParse(cellData.Value.ToString(), out var data))
                            {
                                cell.SetCellValue(data);
                            }
                        }
                        else
                        {
                            cell.SetCellValue(cellData.Value.ToString());
                        }
                    }
                    else
                    {
                        cell.SetCellValue("");
                    }
                }
            }
            // for (int i = 0; i < propertySelectors.Length; i++)
            // {
            //     sheet.AutoSizeColumn(i);
            // }
        }

        protected void AddHeaderStartRowIndex(ISheet sheet, int rowsIndex, int index, params string[] headerTexts)
        {
            if (headerTexts.IsNullOrEmpty())
            {
                return;
            }

            sheet.CreateRow(rowsIndex);

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeaderRowIndexItem(sheet, rowsIndex, index + i, headerTexts[i]);
            }
        }

        protected void AddHeaderRowIndexItem(ISheet sheet, int rowIndex, int columnIndex, string headerText)
        {
            var cell = sheet.GetRow(rowIndex).CreateCell(columnIndex);
            cell.SetCellValue(headerText);
            var cellStyle = sheet.Workbook.CreateCellStyle();
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.FillForegroundColor = HSSFColor.Green.Index;//HSSFColor.Green.Index;
            cellStyle.WrapText = true;
            
            cellStyle.FillPattern = FillPattern.FineDots;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.Alignment = HorizontalAlignment.Center;
            var font = sheet.Workbook.CreateFont();
            font.IsBold = true;
            font.FontHeightInPoints = 12;
            cellStyle.SetFont(font);
            cell.CellStyle = cellStyle;

        }


        protected void AddObjectsRowItemColumn(ISheet sheet, int rowIndex, int columnIndex, string value, bool isNewRow = true, bool isCenter = true)
        {
            if (isNewRow)
                sheet.CreateRow(rowIndex);
            var cell = sheet.GetRow(rowIndex).CreateCell(columnIndex);

            var cellStyle = sheet.Workbook.CreateCellStyle();
            if (isCenter)
            {
                cellStyle.VerticalAlignment = VerticalAlignment.Center;
                cellStyle.Alignment = HorizontalAlignment.Center;
            }
            else 
            {
                cellStyle.VerticalAlignment = VerticalAlignment.Center;
                cellStyle.Alignment = HorizontalAlignment.Left;
            }                 
            var font = sheet.Workbook.CreateFont();
            font.IsBold = true;
            font.FontHeightInPoints = 12;
            cellStyle.SetFont(font);
            cell.CellStyle = cellStyle;
            cell.SetCellValue(value);

        }

        protected void AddObjectsStartRowsIndex<T>(ISheet sheet, int startRowIndex, int columnIndex, ICellStyle style, IList<T> items,
           params Func<T, object>[] propertySelectors)
        {
            //if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            //{
            //    return;
            //}

            for (var i = 1; i <= items.Count; i++)
            {
                var row = sheet.CreateRow(startRowIndex + i);
                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    var cell = row.CreateCell(j + columnIndex);
                    cell.CellStyle = style;
                    var cellObj = propertySelectors[j + columnIndex](items[i - 1]);
                    CellOption cellData;
                    if (cellObj == null)
                    {
                        cellData = CellOption.Create("");
                    }
                    else if (cellObj.GetType() == typeof(CellOption))
                    {
                        cellData = cellObj.ConvertTo<CellOption>();
                    }
                    else
                    {
                        cellData = CellOption.Create(cellObj);
                    }

                    if (cellData != null && cellData.Value != null)
                    {
                        if (cellData.Format.ToLower().StartsWith("dd") ||
                            cellData.Format.ToLower().StartsWith("mm") ||
                            cellData.Format.ToLower().StartsWith("yyyy"))
                        {
                            var dateStyle = cell.Sheet.Workbook.CreateCellStyle();
                            dateStyle.DataFormat =
                                cell.Sheet.Workbook.CreateDataFormat().GetFormat(cellData.Format);
                            cell.CellStyle = dateStyle;
                            if (DateTime.TryParse(cellData.Value.ToString(), out var datetime))
                            {
                                cell.SetCellValue(datetime);
                            }
                        }
                        else if (cellData.Format.ToLower() == "number")
                        {
                            cell.SetCellType(CellType.Numeric);
                            if (double.TryParse(cellData.Value.ToString(), out var data))
                            {
                                cell.SetCellValue(data);
                            }
                        }
                        else
                        {
                            cell.SetCellValue(cellData.Value.ToString());
                        }
                    }
                    else
                    {
                        cell.SetCellValue("");
                    }
                }
            }
        }

        protected void AddObjectsSumRowsIndex<T>(ISheet sheet, int startRowIndex, int columnIndex, ICellStyle style, T items,
          params Func<T, object>[] propertySelectors)
        {
            var row = sheet.CreateRow(startRowIndex);
            for (var j = 0; j < propertySelectors.Length; j++)
            {
                var cell = row.CreateCell(j + columnIndex);
                cell.CellStyle = style;
                var cellObj = propertySelectors[j](items);
                CellOption cellData;
                if (cellObj == null)
                {
                    cellData = CellOption.Create("");
                }
                else if (cellObj.GetType() == typeof(CellOption))
                {
                    cellData = cellObj.ConvertTo<CellOption>();
                }
                else
                {
                    cellData = CellOption.Create(cellObj);
                }

                if (cellData != null && cellData.Value != null)
                {
                    if (cellData.Format.ToLower().StartsWith("dd") ||
                        cellData.Format.ToLower().StartsWith("mm") ||
                        cellData.Format.ToLower().StartsWith("yyyy"))
                    {
                        var dateStyle = cell.Sheet.Workbook.CreateCellStyle();
                        dateStyle.DataFormat =
                            cell.Sheet.Workbook.CreateDataFormat().GetFormat(cellData.Format);
                        cell.CellStyle = dateStyle;
                        if (DateTime.TryParse(cellData.Value.ToString(), out var datetime))
                        {
                            cell.SetCellValue(datetime);
                        }
                    }
                    else if (cellData.Format.ToLower() == "number")
                    {
                        cell.SetCellType(CellType.Numeric);
                        if (double.TryParse(cellData.Value.ToString(), out var data))
                        {
                            cell.SetCellValue(data);
                        }
                    }
                    else
                    {
                        cell.SetCellValue(cellData.Value.ToString());
                    }
                }
                else
                {
                    cell.SetCellValue("");
                }
            }
        }
    }

    public class CellOption
    {
        public object Value { get; set; }
        public string Format { get; set; }

        public static CellOption Create(object value, string format)
        {
            return new CellOption()
            {
                Value = value,
                Format = format,
            };
        }

        public static CellOption Create(object value)
        {
            return new CellOption()
            {
                Value = value,
                Format = "",
            };
        }
    }
}
