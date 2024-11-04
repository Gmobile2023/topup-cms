using System.Collections.Generic;
using Abp.Dependency;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Dto;
using HLS.Topup.Storage;
using TW.CardMapping.Authorization.Users.Importing.Dto;

namespace HLS.Topup.StockManagement.Importing
{
    public class InvalidCardExporter : NpoiExcelExporterBase, IInvalidCardExporter
    {
        public InvalidCardExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<ImportCardDto> cardListDtos)
        {
            return CreateExcelPackage(
                "InvalidUserImportList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("InvalidUserImports"));

                    AddHeader(
                        sheet,
                        L("BatchCode"),
                        L("Serial"),
                        L("CardCode"),
                        L("ExpireDate"),
                        L("CardValue"),
                        L("StockType")
                    );

                    AddObjects(
                        sheet, 2, cardListDtos,
                        _ => _.BatchCode,
                        _ => _.Serial,
                        _ => _.CardCode,
                        _ => _.ExpiredDate,
                        _ => _.CardValue,
                        _ => _.StockType,
                        _ => _.Exception
                    );

                    //for (var i = 0; i < 8; i++)
                    //{
                    //    sheet.AutoSizeColumn(i);
                    //}
                });
        }
    }
}