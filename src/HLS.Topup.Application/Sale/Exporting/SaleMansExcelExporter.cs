using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Sale.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.Sale.Exporting
{
    public class SaleMansExcelExporter : NpoiExcelExporterBase, ISaleMansExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SaleMansExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSaleManForViewDto> saleMans)
        {
            return CreateExcelPackage(
                "SaleMans.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("SaleMans"));

                    AddHeader(
                        sheet,
                        L("UserName"),
                        L("SaleName"),
                        L("PhoneNumber"),
                        L("SaleType"),
                        L("Sale_Leader"),
                        L("SaleCreatedAt"),
                        (L("Status"))
                    );

                    AddObjects(
                        sheet, 2, saleMans,
                        _ => _.SaleMan.UserName,
                        _ => _.SaleMan.FullName,
                        _ => _.SaleMan.PhoneNumber,
                        _ => L("SaleType_" + _.SaleMan.AccountType),
                        _ => _.SaleMan.SaleLeadName,
                        _ => CellOption.Create(_.SaleMan.CreationTime, "dd/MM/yyyy HH:mm:ss"),
                        _ => _.SaleMan.IsActive ? "Kích hoạt" : "Bị khoá"
                    );
                });
        }
    }
}
