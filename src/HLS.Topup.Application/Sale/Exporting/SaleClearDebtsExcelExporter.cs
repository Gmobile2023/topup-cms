using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Sale.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;
using HLS.Topup.Common;

namespace HLS.Topup.Sale.Exporting
{
    public class SaleClearDebtsExcelExporter : NpoiExcelExporterBase, ISaleClearDebtsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SaleClearDebtsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSaleClearDebtForViewDto> saleClearDebts)
        {
            return CreateExcelPackage(
                "SaleClearDebts.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("SaleClearDebts"));

                    AddHeader(
                        sheet,
                       "Mã giao dịch",
                       "Nhân viên",
                       "Số tiền",
                      "Tình trạng",
                      "Mã ngân hàng",
                      "Ngân hàng",
                      "Hình thức thanh toán",
                      "Thời gian duyệt",
                      "Người duyệt",
                      "Thời gian tạo",
                      "Người tạo"
                        );

                    AddObjects(
                        sheet, 2, saleClearDebts,
                        _ => _.SaleClearDebt.TransCode,
                        _ => _.SaleClearDebt.SaleInfo,
                        _ => CellOption.Create(_.SaleClearDebt.Amount, "Number"),
                        _ => _.SaleClearDebt.Status == CommonConst.ClearDebtStatus.Init
                        ? "Chờ duyệt" : _.SaleClearDebt.Status == CommonConst.ClearDebtStatus.Approval
                        ? "Đã duyệt" : "Đã hủy",
                        _ => _.SaleClearDebt.TransCodeBank,
                        _ => _.BankBankName,
                        _ => _.SaleClearDebt.Type == CommonConst.ClearDebtType.CashOnHand ? "Tiền mặt tại quỹ" : "Chuyển khoản",
                        _ => CellOption.Create(_.SaleClearDebt.ModifyDate, "dd/MM/yyyy HH:mm:ss"),
                        _ => _.SaleClearDebt.UserModify,
                         _ => CellOption.Create(_.SaleClearDebt.CreationTime, "dd/MM/yyyy HH:mm:ss"),
                        _ => _.SaleClearDebt.UserCreated
                        );



                });
        }
    }
}
