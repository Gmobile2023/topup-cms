using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.BalanceManager.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.BalanceManager.Exporting
{
    public class SystemAccountTransfersExcelExporter : NpoiExcelExporterBase, ISystemAccountTransfersExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SystemAccountTransfersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSystemAccountTransferForViewDto> systemAccountTransfers)
        {
            return CreateExcelPackage(
                "SystemAccountTransfers.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("SystemAccountTransfers"));

                    AddHeader(
                        sheet,
                        L("TransCode"),
                        L("SrcAccount"),
                        L("DesAccount"),
                        L("Amount"),
                        L("UserCreated"),
                        L("DateCreated"),
                        L("UserApproved"),
                        L("DateApproved"),
                        L("Status")
                    );

                    AddObjects(
                        sheet, 2, systemAccountTransfers,
                        _ => _.SystemAccountTransfer.TransCode,
                        _ => _.SystemAccountTransfer.SrcAccount,
                        _ => _.SystemAccountTransfer.DesAccount,
                        _ => CellOption.Create(_.SystemAccountTransfer.Amount, "Number"),
                        _ => _.UserCreated,
                        _ => CellOption.Create(_.DateCreated, "dd/MM/yyyy HH:mm:ss"),
                        _ => _.UserApproved,
                        _ => CellOption.Create(_.DateApproved, "dd/MM/yyyy HH:mm:ss"),
                        _ => @L("Enum_SystemTransferStatus_" + (byte) _.SystemAccountTransfer.Status)
                    );
                });
        }
    }
}
