using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Sale.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.Sale.Exporting
{
    public class SaleLimitDebtsExcelExporter : NpoiExcelExporterBase, ISaleLimitDebtsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SaleLimitDebtsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSaleLimitDebtForViewDto> saleLimitDebts)
        {
            return CreateExcelPackage(
                "SaleLimitDebts.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("SaleLimitDebts"));

                    AddHeader(
                        sheet,
                        "Nhân viên",
                        "Sale Leader",
                        "Hạn mức công nợ",
                        "Tuổi nợ",
                        "Tình trạng",
                        "Ngày tạo",
                        "Người tạo"
                        );

                    AddObjects(
                        sheet, 2, saleLimitDebts,
                        _ => _.SaleLimitDebt.SaleInfo,
                        _ => _.SaleLimitDebt.SaleLeaderInfo,
                         _ => CellOption.Create(_.SaleLimitDebt.LimitAmount, "Number"),
                         _ => CellOption.Create(_.SaleLimitDebt.DebtAge, "Number"),
                        _ => L("Enum_DebtLimitAmountStatus_" + (_.SaleLimitDebt.Status == Common.CommonConst.DebtLimitAmountStatus.Active ? "1"
                        : _.SaleLimitDebt.Status == Common.CommonConst.DebtLimitAmountStatus.Init ? "0" : "2")),
                            _ => CellOption.Create(_.SaleLimitDebt.CreatedDate, "dd/MM/yyyy HH:mm:ss"),
                            _ => _.SaleLimitDebt.UserCreated
                            );



                });
        }
    }
}
