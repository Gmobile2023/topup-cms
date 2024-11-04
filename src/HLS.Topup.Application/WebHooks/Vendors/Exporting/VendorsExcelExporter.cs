using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Vendors.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.Vendors.Exporting
{
    public class VendorsExcelExporter : NpoiExcelExporterBase, IVendorsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public VendorsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetVendorForViewDto> vendors)
        {
            return CreateExcelPackage(
                "Vendors.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Vendors"));

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name"),
                        L("Description"),
                        L("Print_Help"),
                        L("Print_Suport"),
                        L("Status"),
                        L("Address"),
                        L("HotLine")
                        );

                    AddObjects(
                        sheet, 2, vendors,
                        _ => _.Vendor.Code,
                        _ => _.Vendor.Name,
                        _ => _.Vendor.Description,
                        _ => _.Vendor.Print_Help,
                        _ => _.Vendor.Print_Suport,
                        _ => _.Vendor.Status,
                        _ => _.Vendor.Address,
                        _ => _.Vendor.HotLine
                        );

					
					
                });
        }
    }
}
