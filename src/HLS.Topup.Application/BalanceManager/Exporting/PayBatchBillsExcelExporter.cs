using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.BalanceManager.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.BalanceManager.Exporting
{
    public class PayBatchBillsExcelExporter : NpoiExcelExporterBase, IPayBatchBillsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PayBatchBillsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
       base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPayBatchBillForViewDto> payBatchBills)
        {
            return CreateExcelPackage(
                "PayBatchBills.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("PayBatchBills"));

                    AddHeader(
                        sheet,
                        "Mã chương trình",
                        "Tên chương trình",
                        "Trạng thái",
                        "Số lượng đại lý",
                        "Số lượng giao dịch",
                        "Số tiền tưởng",
                        "Kỳ thanh toán",
                       "Block số lượng hóa đơn",
                        "Số tiền trả cho mỗi Block",
                        "Loại sản phẩm",
                        "Sản phẩm",
                        "Ngày tạo",
                        "Người tạo",
                        "Ngày duyệt",
                        "Người duyệt"
                        );

                    AddObjects(
                        sheet, 2, payBatchBills,
                        _ => _.PayBatchBill.Code,
                        _ => _.PayBatchBill.Name,
                        _ => L("Enum_PayBatchBillStatus_" + ((int)_.PayBatchBill.Status).ToString()),
                           _ => CellOption.Create(_.PayBatchBill.TotalAgent, "Number"),
                        _ => CellOption.Create(_.PayBatchBill.TotalTrans, "Number"),
                        _ => CellOption.Create(_.PayBatchBill.TotalAmount, "Number"),
                        _ => _.PayBatchBill.Period,
                        _ => CellOption.Create(_.PayBatchBill.TotalBlockBill, "Number"),
                        _ => CellOption.Create(_.PayBatchBill.AmountPayBlock, "Number"),
                        _ => _.CategoryName,
                        _ => _.ProductName,
                       _ => CellOption.Create(_.PayBatchBill.CreationTime, "dd/MM/yyyy HH:mm:ss"),
                        _ => _.UserCreated,
                        _ => CellOption.Create(_.PayBatchBill.DateApproved, "dd/MM/yyyy HH:mm:ss"),
                        _ => _.UserApproval
                        );
                });
        }

        public FileDto ExportDetailToFile(List<PayBatchBillItem> payBatchDetails)
        {
            return CreateExcelPackage(
                "Danhsachtrahoahong.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet("Danhsachtrahoahong");
                    AddHeader(
                        sheet,
                        "Mã đại lý",
                        "Số điện thoại",
                        "Họ và tên",
                        "Số lượng giao dịch",
                        "Số tiền thưởng",
                        "Trạng thái",
                        "Mã giao dịch"
                        );

                    AddObjects(
                        sheet, 2, payBatchDetails,
                        _ => _.AgentCode,
                          _ => _.Mobile,
                        _ => _.FullName,                      
                           _ => CellOption.Create(_.Quantity, "Number"),
                        _ => CellOption.Create(_.PayBatchMoney, "Number"),
                         _ => _.StatusName,
                        _ => _.TransRef
                        );
                });
        }
    }
}
