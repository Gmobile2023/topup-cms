using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.BalanceManager.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.BalanceManager.Exporting
{
    public class AccountBlockBalancesExcelExporter : NpoiExcelExporterBase, IAccountBlockBalancesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AccountBlockBalancesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAccountBlockBalanceForViewDto> accountBlockBalances)
        {
            return CreateExcelPackage(
                "AccountBlockBalances.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("AccountBlockBalances"));

                    AddHeader(
                        sheet,
                        "Loại đại lý",
                        "Đại lý",
                        L("BlockMoney"),
                        L("LastModificationTime"),
                        L("CreatorUser")
                    );

                    AddObjects(
                        sheet, 2, accountBlockBalances,
                        _ => @L("Enum_AgentType_" + (byte) _.AgentType),
                        _ => _.FullAgentName,
                        _ => CellOption.Create(_.AccountBlockBalance.BlockedMoney, "Number"),
                        _ => CellOption.Create(_.AccountBlockBalance.LastModificationTime, "dd/MM/yyyy HH:mm:ss"),
                        _ => _.UserName
                    );
                });
        }
    }
}
