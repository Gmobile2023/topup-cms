using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Banks.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.Banks.Exporting
{
    public class BanksExcelExporter : NpoiExcelExporterBase, IBanksExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BanksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBankForViewDto> banks)
        {
            return CreateExcelPackage(
                "Banks.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Banks"));

                    AddHeader(
                        sheet,
                        L("BankName"),
                        L("ShortNameBank"),
                        L("BranchName"),
                        L("BankAccountName"),
                        L("BankAccountCode"),
                        L("BankSmsPhoneNumber"),
                        L("BankSmsGatewayNumber"),
                        L("Status")
                    );

                    AddObjects(
                        sheet, 2, banks,
                        _ => _.Bank.BankName,
                        _ => _.Bank.ShortName,
                        _ => _.Bank.BranchName,
                        _ => _.Bank.BankAccountName,
                        _ => _.Bank.BankAccountCode,
                        _ => _.Bank.SmsPhoneNumber,
                        _ => _.Bank.SmsGatewayNumber,
                        _ => _.Bank.Status
                    );
                });
        }
    }
}