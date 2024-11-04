using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Products.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.Products.Exporting
{
    public class ProductsExcelExporter : NpoiExcelExporterBase, IProductsExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductForViewDto> products)
        {
            return CreateExcelPackage(
                "Products.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Products"));

                    AddHeader(
                        sheet,
                        L("ProductCode"),
                        L("ProductName"),
                        L("Order"),
                        L("ProductValue"),
                        L("ProductType"),
                        L("Status"),
                        L("Unit"),
                        L("CategoryName"),
                        L("CustomerSupportNote"),
                        L("UserManualNote")
                    );

                    AddObjects(
                        sheet, 2, products,
                        _ => _.Product.ProductCode,
                        _ => _.Product.ProductName,
                        _ => _.Product.Order,
                        _ => _.Product.ProductValue,
                        _ => _.Product.ProductType,
                        _ => _.Product.Status,
                        _ => _.Product.Unit,
                        _ => _.CategoryCategoryName,
                        _ => _.Product.CustomerSupportNote,
                        _ => _.Product.UserManualNote
                    );
                });
        }
    }
}