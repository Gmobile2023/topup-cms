using System;
using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Abp.UI;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Transactions;
using HLS.Topup.Storage;

namespace HLS.Topup.Transactions.Exporting
{
    public class TransactionsExcelExporter : NpoiExcelExporterBase, ITransactionsExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TransactionsExcelExporter(ITempFileCacheManager tempFileCacheManager, IAbpSession abpSession,
            ITimeZoneConverter timeZoneConverter) : base(tempFileCacheManager)
        {
            _abpSession = abpSession;
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto TopupDetailExportToFile(List<TopupDetailResponseDTO> topupRequests)
        {
            try
            {
                return CreateExcelPackage(
                    "TransactionDetail.xlsx",
                    excelPackage =>
                    {
                        var sheet = excelPackage.CreateSheet("TransactionDetail");
                        AddHeader(
                            sheet,
                            "Mã giao dịch",
                            "Nhà mạng",
                            "Mệnh giá",
                            "Serial",
                            "Mã thẻ",
                            "Ngày hết hạn",
                            "Số tiền chiết khấu",
                            "Thành tiền"
                        );

                        AddObjects(
                            sheet, 2, topupRequests,
                            _ => _.TransRef,
                            _ => _.Telco,
                            _ => _.CardValue,
                            _ => _.Serial,
                            _ => _.CardCode,
                            _ => _.ExpiredDate.ToString("dd/MM/yyyy"),
                            _ => _.DiscountAmount / _.Quantity,
                            _ => _.PaymentAmount / _.Quantity
                        );
                        // for (var i = 1; i <= topupRequests.Count; i++)
                        // {
                        //     // SetCellAmountFormat(sheet.GetRow(i).Cells[10]);
                        // }
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new UserFriendlyException("Lỗi");
            }
        }


        public FileDto ExportToFile(List<TopupRequestResponseDto> topupRequests, string fileName)
        {
            return CreateExcelPackage(
                fileName != null ? fileName + ".csv" : "Quản lý giao dịch.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("TopupRequests"));

                    AddHeader(
                        sheet,
                        "Loại đại lý",
                        "Mã đại lý",
                        "Nhà cung cấp",
                        L("Status"),
                        "Số thụ hưởng",
                        "Dịch vụ",
                        "Loại sản phẩm",
                        "Tên sản phẩm",
                        "Đơn giá",
                        L("Quantity"),
                        "Tổng chiết khấu",
                        "Phí thanh toán",
                        "Thành tiền",
                        L("CreatedTime"),
                        "Người thực hiện",
                        "Mã giao dịch",
                        "Mã GD đối tác",
                        "Mã GD NCC",
                        "TG bắt đầu",
                        "TG kết thúc",
                        "TG Xử lý (s)"
                    );

                    AddObjects(
                        sheet, 2, topupRequests,
                        _ => L("Enum_AgentType_" + (int)_.AgentType),
                        _ => _.PartnerCode,
                        _ => _.Provider,
                        _ => L("Enum_TopupStatus_" + (int)_.Status),
                        _ => _.ReceiverInfo,
                        _ => _.ServiceName,
                        _ => _.CategoryCode,
                        _ => _.ProductCode,
                        _ => CellOption.Create(_.Price, "Number"),
                        _ => CellOption.Create(_.Quantity, "Number"),
                        _ => CellOption.Create(_.DiscountAmount, "Number"),
                        _ => CellOption.Create(_.Fee, "Number"),
                        _ => CellOption.Create(_.PaymentAmount, "Number"),
                        _ => _.CreatedTime,
                        _ => _.StaffAccount,
                        _ => _.TransCode,
                        _ => _.TransRef,
                        _ => _.ProviderTransCode,
                        _ => _.ResponseDate ?? DateTime.Now,
                        _ => _.RequestDate ?? _.CreatedTime,
                        _ => _.TotalTime
                    );

                    for (var i = 1; i <= topupRequests.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[13], "dd-mm-yyyy hh:mm:ss");
                        SetCellDataFormat(sheet.GetRow(i).Cells[18], "dd-mm-yyyy hh:mm:ss");
                        SetCellDataFormat(sheet.GetRow(i).Cells[19], "dd-mm-yyyy hh:mm:ss");
                    }
                });
        }

        public FileDto ExportToFilePartner(List<TopupRequestResponseDto> topupRequests)
        {
            return CreateExcelPackage(
                "TransactionHistory.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("TopupRequests"));
                    AddHeader(
                        sheet,
                        "Mã giao dịch",
                        "Mã đại lý",
                        "Loại giao dịch",
                        "Nhà cung cấp",
                        "Mệnh giá",
                        "Số lượng",
                        "Chiết khấu",
                        "Sô tiền thanh toán",
                        "Tài khoản thụ hưởng",
                        "Trạng thái",
                        "Ngày tạo",
                        "NV thực hiện"
                    );

                    AddObjects(
                        sheet, 2, topupRequests,
                        _ => _.TransRef,
                        _ => _.PartnerCode,
                        _ => _.ServiceName,
                        _ => _.ProviderName,
                        _ => _.Price,
                        _ => _.Quantity,
                        _ => _.DiscountAmount.ToString("####"),
                        _ => _.PaymentAmount.ToString("####"),
                        _ => _.ReceiverInfo,
                        _ => _.StatusName,
                        _ => _timeZoneConverter.Convert(_.CreatedTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.StaffAccount
                    );
                    for (var i = 1; i <= topupRequests.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[10], "dd-mm-yyyy hh:mm:ss");
                    }
                });
        }

        public FileDto ExportItemsToFile(List<TopupDetailResponseDTO> topupRequests)
        {
            return CreateExcelPackage(
                "TopupRequests.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("TopupRequests"));
                    AddHeader(
                        sheet,
                        L("TransCode"),
                        "Mã giao dịch đối tác",
                        "Loại giao dịch",
                        L("Status"),
                        L("Quantity"),
                        L("Đơn giá"),
                        "Cần nạp",
                        "Đã nạp",
                        "Đã hủy",
                        "Thanh toán",
                        L("CreatedTime"),
                        "Nhà mạng",
                        "Số tiền chiết khấu",
                        "Đua giá",
                        "Nhà cung cấp",
                        "Sản phẩm",
                        L("MobileNumber"),
                        L("PartnerCode"),
                        "Serial",
                        "Loại giao dịch",
                        "WorkerApp"
                    );

                    AddObjects(
                        sheet, 2, topupRequests,
                        _ => _.TransCode,
                        _ => _.TransRef,
                        _ => _.ServiceName,
                        _ => _.StatusName,
                        _ => _.Quantity,
                        _ => _.ItemAmount,
                        _ => _.Amount,
                        _ => _.ProcessedAmount,
                        _ => _.CancelAmount,
                        _ => _.PaymentAmount,
                        _ => _timeZoneConverter.Convert(_.CreatedTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        //_ => _.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        _ => _.Telco,
                        _ => _.DiscountAmount,
                        _ => _.PriorityDiscountRate,
                        _ => _.Provider,
                        _ => _.ProductCode,
                        _ => _.ReceiverInfo,
                        _ => _.PartnerCode,
                        _ => _.Serial,
                        _ => _.TopupTransactionType,
                        _ => _.WorkerApp
                    );
                });
        }

        public FileDto TopupDetailRequestExportToFile(List<TopupDetailResponseDTO> topupDetailResponse, string fileName)
        {
            return CreateExcelPackage(
                fileName != null ? "Chi tiet giao dich " + fileName + ".xlsx" : "Chi tiet giao dich mua ma the.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("TopupRequests"));

                    AddHeader(
                        sheet,
                        "Mệnh giá",
                        "Serial",
                        "Ngày hết hạn"
                    );

                    AddObjects(
                        sheet, 2, topupDetailResponse,
                        _ => CellOption.Create(_.CardValue, "Number"),
                        _ => _.Serial,
                        _ => _.ExpiredDate.ToString("dd/MM/yyyy")
                    );
                });
        }

        public FileDto BatchLotExportToFile(List<BatchItemDto> items)
        {
            return CreateExcelPackage(
                "LichSuNapLo.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Sheet1");

                    AddHeader(
                        sheet,
                        "Trạng thái",
                        "Mã lô",
                        "Loại",
                        "Người thực hiện",
                        "Ngày tạo",
                        "Ngày cập nhật"
                    );

                    AddObjects(
                        sheet, 2, items,
                        _ => _.StatusName,
                        _ => _.BatchCode,
                        _ => _.BatchName,
                        _ => _.StaffAccount,
                        _ => _.CreatedTime.ToString("dd/MM/yyyy HH:mm:ss"),
                        _ => _.EndProcessTime.ToString("dd/MM/yyyy HH:mm:ss")
                    );
                });
        }

        public FileDto BatchLotTopupDetailExportToFile(List<BatchDetailDto> items)
        {
            return CreateExcelPackage(
                "LichSuNapLoChitiet.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Sheet1");
                    AddHeader(
                        sheet,
                        "Trạng thái",
                        "Số điện thoại",
                        "Nhà mạng",
                        "Mệnh giá",
                        "Chiết khấu",
                        "Thành tiền",
                        "Trạng thái giao dịch",
                        "Ngày cập nhật"
                    );

                    AddObjects(
                        sheet, 2, items,
                        _ => _.BatchName,
                        _ => _.ReceiverInfo,
                        _ => _.CategoryName,
                        _ => _.Amount,
                        _ => _.DiscountAmount,
                        _ => _.PaymentAmount,
                        _ => _.StatusName,
                        _ => _.UpdateTime != null ? _.UpdateTime.Value.ToString("dd/MM/yyyy HH:mm:ss") : ""
                    );
                });
        }

        public FileDto BatchLotBillDetailExportToFile(List<BatchDetailDto> items)
        {
            return CreateExcelPackage(
                "LichSuNapLoChitiet.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Sheet1");
                    AddHeader(
                        sheet,
                        "Trạng thái",
                        "Hóa đơn điện",
                        "Nhà cung cấp",
                        "Mã khách hàng",
                        "Số tiền",
                        "Chiết khấu",
                        "Phí",
                        "Thành tiền",
                        "Trạng thái giao dịch",
                        "Ngày cập nhật"
                    );

                    AddObjects(
                        sheet, 2, items,
                        _ => _.BatchName,
                        _ => _.ProductName,
                        _ => _.Provider,
                        _ => _.ReceiverInfo,
                        _ => _.Amount,
                        _ => _.DiscountAmount,
                        _ => _.Fee,
                        _ => _.PaymentAmount,
                        _ => _.StatusName,
                        _ => _.UpdateTime != null ? _.UpdateTime.Value.ToString("dd/MM/yyyy HH:mm:ss") : ""
                    );
                });
        }

        public FileDto BatchLotPinCodeDetailExportToFile(List<BatchDetailDto> items)
        {
            return CreateExcelPackage(
                "LichSuNapLoChitiet.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Sheet1");
                    AddHeader(
                        sheet,
                        "Trạng thái",
                        "Loại thẻ",
                        "Nhà phát hành",
                        "Mệnh giá",
                        "Số lượng",
                        "Chiết khấu",
                        "Thành tiền",
                        "Trạng thái giao dịch",
                        "Ngày cập nhật"
                    );

                    AddObjects(
                        sheet, 2, items,
                        _ => _.BatchName,
                        _ => _.ServiceName,
                        _ => _.CategoryName,
                        _ => _.Amount,
                        _ => _.Quantity,
                        _ => _.DiscountAmount,
                        _ => _.PaymentAmount,
                        _ => _.StatusName,
                        _ => _.UpdateTime != null ? _.UpdateTime.Value.ToString("dd/MM/yyyy HH:mm:ss") : ""
                    );
                });
        }


        public FileDto OffsetTopupExportToFile(List<SaleOffsetReponseDto> items)
        {
            return CreateExcelPackage(
                "Offset.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Sheet1");

                    AddHeader(
                        sheet,
                        "Trạng thái",
                        "Đơn giá",
                        "Số thụ hưởng",
                        "Thời gian",
                        "Mã đại lý nạp chậm",
                        "Mã đại lý nạp bù",
                        "MGD nạp chậm",
                        "NCC nạp chậm",
                        "MGD nạp bù"
                    );

                    AddObjects(
                        sheet, 2, items,
                        _ => _.StatusName,
                        _ => _.Amount,
                        _ => _.ReceiverInfo,
                        _ => _.CreatedTime.ToString("dd/MM/yyyy HH:mm:ss"),
                      _ => _.OriginPartnerCode,
                      _ => _.PartnerCode,
                      _ => _.OriginTransCode,
                      _ => _.OriginProviderCode,
                      _ => _.TransCode
                    );
                });
        }
    }
}
