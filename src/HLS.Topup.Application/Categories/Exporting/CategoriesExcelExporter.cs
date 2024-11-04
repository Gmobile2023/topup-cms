using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.Categories.Exporting
{
    public class CategoriesExcelExporter : NpoiExcelExporterBase, ICategoriesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCategoryForViewDto> categories)
        {
            return CreateExcelPackage(
                "Categories.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Categories"));

                    AddHeader(
                        sheet,
                        L("CategoryCode"),
                        L("CategoryName"),
                        L("Order"),
                        L("Status"),
                        L("Type"),
                        L("CategoryName"),
                        L("ServicesName")
                    );

                    AddObjects(
                        sheet, 2, categories,
                        _ => _.Category.CategoryCode,
                        _ => _.Category.CategoryName,
                        _ => _.Category.Order,
                        _ => _.Category.Status,
                        _ => _.Category.Type,
                        _ => _.CategoryCategoryName,
                        _ => _.ServiceServicesName
                    );
                });
        }
    }
}