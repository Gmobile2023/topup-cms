using System;
using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Dto;
using HLS.Topup.Report;
using HLS.Topup.Reports.Dtos;
using HLS.Topup.Storage;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using static HLS.Topup.Report.ReportComparePartnerExportInfo;

namespace HLS.Topup.Reports.Exporting
{
    public partial class ReportExcelExporter : NpoiExcelExporterBase, IReportExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ReportExcelExporter(ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ReportDetailExportToFile(List<ReportDetailDto> input)
        {
            string fileName = string.Format("Bao cao chi tiet so du tai khoan_{0}.xlsx", DateTime.Now.Date.ToString("ddMMyyy"));
            return CreateExcelPackage(
                fileName,
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("BalanceAccount");
                    AddHeader(
                        sheet,
                        "Mã giao dịch",
                        "Loại giao dịch",
                        "Ngày giao dịch",
                        "Số dư trước giao dịch",
                        "Phát sinh tăng",
                        "Phát sinh giảm",
                        "Số dư sau giao dịch",
                        "Nội dung"
                    );


                    AddObjects(
                        sheet, 2, input,
                        _ => _.TransCode,
                        _ => _.ServiceName,
                        _ => CellOption.Create(_.CreatedDate, "dd/MM/yyyy HH:mm:ss"),
                        _ => CellOption.Create(_.BalanceBefore, "Number"),
                        _ => CellOption.Create(_.Increment, "Number"),
                        _ => CellOption.Create(_.Decrement, "Number"),
                        _ => CellOption.Create(_.BalanceAfter, "Number"),
                        _ => _.TransNote
                    );
                });
        }

        public FileDto ReportTotalExportToFile(List<ReportTotalDto> input)
        {
            string fileName = string.Format("Bao cao tong hop so du tai khoan_{0}.xlsx", DateTime.Now.ToString("ddMMyyyy"));
            return CreateExcelPackage(
                fileName,
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Report");
                    AddHeader(
                        sheet,
                        "Loại đại lý",
                        "Đại lý",
                        "Số dư đầu kỳ",
                        "Phát sinh tăng",
                        "Phát sinh giảm",
                        "Số dư cuối kỳ"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.AgentType == 1
                        ? "Đại lý" : _.AgentType == 2
                        ? "Đại lý API" : _.AgentType == 3
                        ? "Đại lý công ty" : _.AgentType == 4
                        ? "Đại lý Tổng" : _.AgentType == 5
                        ? "Đại lý cấp 1" : _.AgentType == 6
                        ? "Đại lý sỉ" : "Đại lý",
                        _ => _.AccountInfo,
                        _ => CellOption.Create(_.BalanceBefore, "Number"),
                        _ => CellOption.Create(_.Credited, "Number"),
                        _ => CellOption.Create(_.Debit, "Number"),
                        _ => CellOption.Create(_.BalanceAfter, "Number")
                    );
                });
        }

        public FileDto ReportGroupExportToFile(List<ReportGroupDto> input)
        {
            return CreateExcelPackage(
                "TotalBalance.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("TotalBalance");
                    //sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        "Mã tài khoản",
                        "Số dư đầu kỳ",
                        "Phát sinh tăng",
                        "Phát sinh giảm",
                        "Số dư cuối kỳ"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.AccountCode,
                        _ => CellOption.Create(_.BalanceBefore, "Number"),
                        _ => CellOption.Create(_.Credited, "Number"),
                        _ => CellOption.Create(_.Debit, "Number"),
                        _ => CellOption.Create(_.BalanceAfter, "Number")
                    );
                });
        }

        public FileDto ReportTransDetailExportToFile(List<ReportTransDetailDto> input)
        {
            return CreateExcelPackage(
                "ReportTransDetail.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Detail");
                    AddHeader(
                        sheet,
                        "Trạng thái",
                        "Loại giao dịch",
                        "Nhà cung cấp",
                        "Đơn giá",
                        "Số lượng",
                        "Chiết khấu",
                        "Phí",
                        "Thu",
                        "Chi",
                        "Số dư",
                        "Tài khoản thụ hưởng",
                        "Mã giao dịch",
                        "Người thực hiện",
                        "Thời gian",
                        "Mã tham chiếu"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.StatusName,
                        _ => _.TransTypeName,
                        _ => _.Vender,
                        _ => Convert.ToDouble(_.Amount),
                        _ => CellOption.Create(_.Quantity, "Number"),
                        _ => CellOption.Create(_.Discount, "Number"),
                        _ => CellOption.Create(_.Fee, "Number"),
                        _ => CellOption.Create(_.PriceIn, "Number"),
                        _ => CellOption.Create(_.PriceOut, "Number"),
                        _ => CellOption.Create(_.Balance, "Number"),
                        _ => _.AccountRef,
                        _ => _.TransCode,
                        _ => _.UserProcess,
                        _ => CellOption.Create(_.CreatedDate, "dd/MM/yyyy HH:mm:ss"),
                        _ => _.RequestTransSouce
                    );
                });
        }

        public FileDto ReportDebtDetailExportToFile(List<ReportDebtDetailDto> input)
        {
            return CreateExcelPackage(
                "Excel.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Detail");
                    AddHeader(
                        sheet,
                        "Thời gian",
                        "Mã giao dịch",
                        "Loại công nợ",
                        "Nội dung",
                        "Số tiền phát sinh nợ",
                        "Số tiền thanh toán",
                        "Hạn mức còn lại"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => CellOption.Create(_.CreatedTime, "dd/MM/yyyy HH:mm:ss"),
                        _ => _.TransCode,
                        _ => _.ServiceName,
                        _ => _.Description,
                        _ => CellOption.Create(_.DebitAmount, "Number"),
                        _ => CellOption.Create(_.CreditAmount, "Number"),
                        _ => CellOption.Create(_.Balance, "Number")
                    );
                });
        }
        public FileDto ReportRefundDetailExportToFile(List<ReportRefundDetailDto> input)
        {
            return CreateExcelPackage(
                "Bao cao chi tiet hoan tien.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Report");
                    AddHeader(
                        sheet,
                        "Mã đại lý",
                        "Tên cửa hàng",
                        "Mã giao dịch",
                        "Dịch vụ",
                        "Loại sản phẩm",
                        "Sản phẩm",
                        "Số tiền",
                        "Mã giao dịch gốc",
                        "Thời gian"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.AgentInfo,
                        _ => _.AgentName,
                        _ => _.TransCode,
                        _ => _.ServiceName,
                        _ => _.CategoryName,
                        _ => _.ProductName,
                        _ => CellOption.Create(_.Price, "Number"),
                        _ => _.TransCodeSouce,
                        _ => CellOption.Create(_.CreatedTime, "dd/MM/yyyy HH:mm:ss")
                    );
                });
        }
        public FileDto ReportTransferDetailExportToFile(List<ReportTransferDetailDto> input)
        {
            return CreateExcelPackage(
                "Bao cao chuyển tiền đại lý.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Sheet1");
                    AddHeader(
                        sheet,
                        "Loại đại lý",
                        "Đại lý chuyển tiền",
                        "Đại lý nhận tiền",
                        "Số tiền",
                        "Thời gian giao dịch",
                        "Ghi chú"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.AgentTypeName,
                        _ => _.AgentTransferInfo,
                        _ => _.AgentReceiveInfo,
                        _ => CellOption.Create(_.Price, "Number"),
                        _ => CellOption.Create(_.CreatedTime, "dd/MM/yyyy HH:mm:ss"),
                        _ => _.Messager
                    );
                });
        }
        public FileDto ReportServiceDetailExportToFile(List<ReportServiceDetailDto> input)
        {
            return CreateExcelPackage(
                "Bao cao ban hang chi tiet.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Detail");
                    AddHeader(
                        sheet,
                        "Loại đại lý",
                        "Mã đại lý",
                        "NVKD",
                        "Nhà cung cấp",
                        "Dịch vụ",
                        "Loại sản phẩm",
                        "Tên sản phẩm",
                        "Đơn giá",
                        "Số lượng",
                        "Số tiền chiết khấu",
                        "Phí",
                        "Thành tiền",
                        "Hoa hồng ĐL tổng",
                        "Đại lý tổng",
                        "Số thụ hưởng",
                        "Thời gian",
                        "Trạng thái",
                        "Người thực hiện",
                        "Mã giao dịch",
                        "Mã đối tác",
                        "Mã nhà cung cấp",
                        "Kênh",
                        "Loại thuê bao",                        
                        "Nhà cung cấp trả về",
                        "Loại thuê bao NCC trả về",
                        "Nhà cung cấp cha"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.AgentTypeName,
                        _ => _.AgentInfo,
                        _ => _.StaffInfo,
                        _ => _.VenderName,
                        _ => _.ServiceName,
                        _ => _.CategoryName,
                        _ => _.ProductName,
                        _ => CellOption.Create(_.Value, "Number"),
                        _ => CellOption.Create(_.Quantity, "Number"),
                        _ => CellOption.Create(_.Discount, "Number"),
                        _ => CellOption.Create(_.Fee, "Number"),
                        _ => CellOption.Create(_.Price, "Number"),
                        _ => CellOption.Create(_.CommistionAmount, "Number"),
                        _ => _.AgentParentInfo,
                        _ => _.ReceivedAccount,
                        _ => CellOption.Create(_.CreatedTime, "dd/MM/yyyy HH:mm:ss"),
                        _ => _.StatusName,
                        _ => _.UserProcess,
                        _ => _.TransCode,
                        _ => _.RequestRef,
                        _ => _.PayTransRef,
                        _ => _.Channel,
                        _ => _.ReceiverType,
                        _ => _.ProviderTransCode,
                        _ => _.ProviderReceiverType,
                        _ => _.ParentProvider
                    );
                });
        }

        public FileDto ReportServiceTotalExportToFile(List<ReportServiceTotalDto> input)
        {
            return CreateExcelPackage(
                "Bao cao tong hop xuat ban theo san pham.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Report");
                    AddHeader(
                        sheet,
                        "Dịch vụ",
                        "Loại sản phẩm",
                        "Tên sản phẩm",
                        "Số lượng",
                        "Tổng tiền chiết khấu",
                        "Phí",
                        "Thành tiền trừ chiết khấu"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.ServiceName,
                        _ => _.CategoryName,
                        _ => _.ProductName,
                        _ => CellOption.Create(_.Quantity, "Number"),
                        _ => CellOption.Create(_.Discount, "Number"),
                        _ => CellOption.Create(_.Fee, "Number"),
                        _ => CellOption.Create(_.Price, "Number")
                    );
                });
        }


        public FileDto ReportServiceProviderExportToFile(List<ReportServiceProviderDto> input)
        {
            return CreateExcelPackage(
                "Bao cao ban hang tong hop.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Report");
                    AddHeader(
                        sheet,
                        "Dịch vụ",
                        "Loại sản phẩm",
                        "Sản phẩm",
                        "Số lượng",
                        "Tổng mệnh giá",
                        "Chiết khấu",
                        "Phí",
                        "Thành tiền",
                        "Nhà cung cấp"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.ServiceName,
                        _ => _.CategoryName,
                        _ => _.ProductName,
                        _ => CellOption.Create(_.Quantity, "Number"),
                        _ => CellOption.Create(_.Value, "Number"),
                        _ => CellOption.Create(_.Discount, "Number"),
                        _ => CellOption.Create(_.Fee, "Number"),
                        _ => CellOption.Create(_.Price, "Number"),
                        _ => _.ProviderName
                    );
                });
        }


        public FileDto ReportAgentBalanceExportToFile(List<ReportAgentBalanceDto> input)
        {
            return CreateExcelPackage(
                "Bao cao NXT tien nap.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Report");
                    AddHeader(
                        sheet,
                        "Loại đại lý",
                        "Đại lý",
                        "Nhân viên sale",
                        "Sale Leader",
                        "Số dư đầu kỳ",
                        "Nạp tiền",
                        "Phát sinh tăng khác",
                        "Bán hàng",
                        "Phát sinh giảm khác",
                        "Số dư cuối kỳ"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.AgentTypeName,
                        _ => _.AgentInfo,
                        _ => _.SaleInfo,
                        _ => _.SaleLeaderInfo,
                        _ => CellOption.Create(_.BeforeAmount, "Number"),
                        _ => CellOption.Create(_.InputAmount, "Number"),
                        _ => CellOption.Create(_.AmountUp, "Number"),
                        _ => CellOption.Create(_.SaleAmount, "Number"),
                        _ => CellOption.Create(_.AmountDown, "Number"),
                        _ => CellOption.Create(_.AfterAmount, "Number")
                    );
                });
        }

        public FileDto ReportRevenueAgentExportToFile(List<ReportRevenueAgentDto> input)
        {
            return CreateExcelPackage(
                "Bao cao doanh so theo dai ly.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Report");
                    AddHeader(
                        sheet,
                        "Loại đại lý",
                        "Đại lý",
                        "Tên cửa hàng",
                        "Nhân viên Sale",
                        "Sale Leader",
                        "Tỉnh/TP",
                        "Quận/Huyện",
                        "Phường/Xã",
                        "Số lượng giao dịch",
                        "Thành tiền sau khi trừ chiết khấu"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.AgentTypeName,
                        _ => _.AgentInfo,
                        _ => _.AgentName,
                        _ => _.SaleInfo,
                        _ => _.SaleLeaderInfo,
                        _ => _.CityInfo,
                        _ => _.DistrictInfo,
                        _ => _.WardInfo,
                        _ => CellOption.Create(_.Quantity, "Number"),
                        _ => CellOption.Create(_.Price, "Number")
                    );
                });
        }

        public FileDto ReportRevenueCityExportToFile(List<ReportRevenueCityDto> input)
        {
            return CreateExcelPackage(
                "Bao cao doanh so theo tinh thanh pho.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Report");
                    AddHeader(
                        sheet,
                        "Tỉnh/Thành phố",
                        "Quận/Huyện",
                        "Phường/Xã",
                        "Số lượng đại lý bán hàng",
                        "Số lượng",
                        "Tổng tiền chiết khấu",
                        "Phí",
                        "Thành tiền sau khi trừ chiết khấu"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.CityInfo,
                        _ => _.DistrictInfo,
                        _ => _.WardInfo,
                        _ => CellOption.Create(_.QuantityAgent, "Number"),
                        _ => CellOption.Create(_.Quantity, "Number"),
                        _ => CellOption.Create(_.Discount, "Number"),
                        _ => CellOption.Create(_.Fee, "Number"),
                        _ => CellOption.Create(_.Price, "Number")
                    );
                });
        }

        public FileDto ReportTotalSaleAgentExportToFile(List<ReportTotalSaleAgentDto> input)
        {
            return CreateExcelPackage(
                "Bao cao tong hop ban the theo dai ly.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Report");
                    AddHeader(
                        sheet,
                        "Loại đại lý",
                        "Đại lý",
                        "Tên cửa hàng",
                        "Nhân viên Sale",
                        "Sale Leader",
                        "Tỉnh/Thành phố",
                        "Quận/Huyện",
                         "Phường/Xã",
                        "Số lượng thẻ",
                        "Tổng tiền chiết khấu",
                        "Thành tiền sau khi trừ chiết khấu"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.AgentTypeName,
                        _ => _.AgentInfo,
                        _ => _.AgentName,
                        _ => _.SaleInfo,
                        _ => _.SaleLeaderInfo,
                        _ => _.CityInfo,
                        _ => _.DistrictInfo,
                        _ => _.WardInfo,
                        _ => CellOption.Create(_.Quantity, "Number"),
                        _ => CellOption.Create(_.Discount, "Number"),
                        _ => CellOption.Create(_.Price, "Number")
                    );
                });
        }

        public FileDto ReportRevenueActiveExportToFile(List<ReportRevenueActiveDto> input)
        {
            return CreateExcelPackage(
                "Bao cao dai ly Active.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Report");
                    AddHeader(
                        sheet,
                        "Loại đại lý",
                        "Đại lý",
                        "Tên cửa hàng",
                        "Nhân viên quản lý",
                        "Sale Leader",
                        "Tỉnh/TP",
                        "Quận/Huyện",
                        "Phường/Xã",
                        "Số giấy tờ",
                        "Tổng số tiền nạp trong kỳ",
                        "Tổng số tiền bán hàng trong kỳ",
                        "Trạng thái"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.AgentTypeName,
                        _ => _.AgentInfo,
                        _ => _.AgentName,
                        _ => _.SaleInfo,
                        _ => _.SaleLeaderInfo,
                        _ => _.CityInfo,
                        _ => _.DistrictInfo,
                        _ => _.WardInfo,
                        _ => _.IdIdentity,
                        _ => CellOption.Create(_.Deposit, "Number"),
                        _ => CellOption.Create(_.Sale, "Number"),
                        _ => _.Status
                    );
                });
        }

        public FileDto ReportTotalDayExportToFile(List<ReportItemTotalDay> input)
        {
            return CreateExcelPackage(
                "ReportTotalDay.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Total");
                    AddHeader(
                        sheet,
                        "Ngày",
                        "Số dư đầu kỳ",
                        "Phát sinh tăng:Nạp tiền",
                        "Phát sinh tăng:Khác",
                        "Phát sinh giảm:Bán hàng",
                        "Phát sinh giảm:Khác",
                         "Số dư cuối kỳ"
                    );
                    AddObjects(
                        sheet, 2, input,
                        _ => CellOption.Create(_.CreatedDay, "dd/MM/yyyy"),
                        _ => CellOption.Create(_.BalanceBefore, "Number"),
                        _ => CellOption.Create(_.IncDeposit, "Number"),
                        _ => CellOption.Create(_.IncOther, "Number"),
                        _ => CellOption.Create(_.DecPayment, "Number"),
                        _ => CellOption.Create(_.DecOther, "Number"),
                        _ => CellOption.Create(_.BalanceAfter, "Number")
                    );
                });
        }

        public FileDto ReportTotalDebtExportToFile(List<ReportItemTotalDebt> input)
        {
            return CreateExcelPackage(
                "Bao cao tong hop cong no.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Report");
                    AddHeader(
                        sheet,
                        "Nhân viên",
                        "Hạn mức đầu kỳ",
                        "Phát sinh nợ trong kỳ:Công nợ phát sinh",
                        "Phát sinh nợ trong kỳ:Thanh toán công nợ",
                         "Hạn mức cuối kỳ"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.SaleInfo,
                        _ => CellOption.Create(_.BalanceBefore, "Number"),
                        _ => CellOption.Create(_.DecPayment, "Number"),
                        _ => CellOption.Create(_.IncDeposit, "Number"),
                        _ => CellOption.Create(_.BalanceAfter, "Number")
                    );
                });
        }

        public FileDto ReportCardStockHistoriesToFile(List<ReportCardStockHistoriesDto> input)
        {
            return CreateExcelPackage(
                "ReportCardStockHistories.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("ReportCardStockHistories"));
                    //sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        "Ngày phát sinh",
                        L("StockCode"),
                        L("Telco"),
                        L("CardValue"),
                        "Tồn đầu kỳ",
                        "Nhập trong kỳ",
                        "Xuất trong kỳ",
                        "Tồn cuối kỳ",
                        "Loại",
                        "Serial"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _timeZoneConverter.Convert(_.CreatedDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.StockCode,
                        _ => _.Telco,
                        _ => _.CardValue,
                        _ => _.InventoryBefore,
                        _ => _.Increase,
                        _ => _.Decrease,
                        _ => _.InventoryAfter,
                        _ => _.InventoryType,
                        _ => _.Serial
                    );
                });
        }

        public FileDto ReportCardStockImExPortToFile(List<ReportCardStockImExPortDto> input)
        {
            string fileName = string.Format("Bao cao NXT San pham_{0}.xlsx", DateTime.Now.ToString("ddMMyyyy"));
            return CreateExcelPackage(
                fileName,
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Sheet1");
                    AddHeader(
                        sheet,
                        "Dich vụ",
                        "Loại sản phẩm",
                        "Mệnh giá",
                        "Tồn đầu",
                        "Nhập từ nhà CC",
                        "Nhập từ kho khác",
                        "Xuất bán",
                        "Xuất kho khác",
                        "Tồn cuối",
                        "Tồn hiện tại"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.ServiceName,
                        _ => _.CategoryName,
                        _ => CellOption.Create(_.CardValue, "Number"),
                        _ => CellOption.Create(_.Before, "Number"),
                        _ => CellOption.Create(_.IncreaseSupplier, "Number"),
                        _ => CellOption.Create(_.IncreaseOther, "Number"),
                        _ => CellOption.Create(_.Sale, "Number"),
                        _ => CellOption.Create(_.ExportOther, "Number"),
                        _ => CellOption.Create(_.After, "Number"),
                        _ => CellOption.Create(_.Current, "Number")
                    );
                });
        }

        public FileDto ReportCardStockAutoToFile(List<ReportCardStockAutoDto> input, string date)
        {
            return CreateExcelPackage(
               string.Format("Bao cao NXT kho the_{0}.xlsx", date),
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Sheet1");
                    AddHeader(
                        sheet,
                        "Dịch vụ",
                        "Loại sản phẩm",
                        "Sản phẩm",
                        "Mệnh giá",
                        "Kho Temp/Tồn đầu kỳ",
                        "Kho Temp/SL Nhập",
                        "Kho Temp/SL Xuất",
                        "Kho Temp/Tồn cuối kỳ",
                        "Kho Temp/Thành tiền tồn cuối",
                        "Kho Sale/Tồn đầu kỳ",
                        "Kho Sale/SL Nhập",
                        "Kho Sale/SL Xuất",
                        "Kho Sale/Tồn cuối kỳ",
                        "Kho Sale/Thành tiền tồn cuối"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.ServiceName,
                        _ => _.CategoryName,
                        _ => _.ProductName,
                        _ => CellOption.Create(_.CardValue, "Number"),
                        _ => CellOption.Create(_.Before_Temp, "Number"),
                        _ => CellOption.Create(_.Import_Temp, "Number"),
                        _ => CellOption.Create(_.Export_Temp, "Number"),
                        _ => CellOption.Create(_.After_Temp, "Number"),
                        _ => CellOption.Create(_.Monney_Temp, "Number"),
                        _ => CellOption.Create(_.Before_Sale, "Number"),
                        _ => CellOption.Create(_.Import_Sale, "Number"),
                        _ => CellOption.Create(_.Export_Sale, "Number"),
                        _ => CellOption.Create(_.After_Sale, "Number"),
                        _ => CellOption.Create(_.Monney_Sale, "Number")
                    );
                });
        }

        public FileDto ReportCardStockInventoryToFile(List<ReportCardStockInventoryDto> input)
        {
            return CreateExcelPackage(
                $"ReportCardStockInventory_{DateTime.Now.ToShortDateString()}.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("ReportCardStockInventory"));
                    //sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        "Ngày phát sinh",
                        L("StockCode"),
                        "Loại kho",
                        "Sản phẩm",
                        "Tồn đầu kỳ",
                        "Nhập trong kỳ",
                        "Xuất trong kỳ",
                        "Tồn cuối kỳ"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _timeZoneConverter.Convert(_.CreatedDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.StockCode,
                        _ => _.StockType,
                        _ => _.ProductCode,
                        _ => _.InventoryBefore,
                        _ => _.Increase,
                        _ => _.Decrease,
                        _ => _.InventoryAfter
                    );
                });
        }


        public FileDto ExportAutoCampareStockToFile(List<ReportCardStockInventoryDto> cardStocks)
        {
            return CreateExcelPackage(
                $"AutoCampareCardStock_{DateTime.Now.ToShortDateString()}.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("AutoCampareCardStock");
                    //sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("StockCode"),
                        "Loại kho",
                        "Sản phẩm",
                        "Tồn đầu kỳ",
                        "Nhập trong kỳ",
                        "Xuất trong kỳ",
                        "Xuất tính theo GD",
                        "Tồn cuối kỳ",
                        "Chênh lệch theo kho",
                        "Chênh lệch theo giao dịch"
                    );

                    AddObjects(
                        sheet, 2, cardStocks,
                        _ => _.StockCode,
                        _ => _.StockType,
                        _ => _.ProductCode,
                        _ => _.InventoryBefore,
                        _ => _.Increase,
                        _ => _.Decrease,
                        _ => _.ExportForTrans,
                        _ => _.InventoryAfter,
                        _ => _.CampareInventory,
                        _ => _.CampareTrans
                   );
                });
        }

        public FileDto ExportRevenueDashboardListExportToFile(List<ReportRevenueDashboardDay> input)
        {
            return CreateExcelPackage(
                "Bao cao tong hop doanh so.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("sheet1");
                    AddHeader(
                        sheet,
                        "Ngày",
                        "Doanh số",
                        "Chiết khấu"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => CellOption.Create(_.CreatedDay, "dd/MM/yyyy"),
                        _ => CellOption.Create(_.Revenue, "Number"),
                        _ => CellOption.Create(_.Discount, "Number")
                    );
                });
        }

        public FileDto ExportAgentGeneralCommistionDashListExportToFile(List<ReportRevenueCommistionDashDay> input)
        {
            return CreateExcelPackage(
                "Bao cao thong hop doanh so dai ly cap 1.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("sheet1");
                    AddHeader(
                        sheet,
                        "Ngày",
                        "Doanh số",
                        "Hoa hồng"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => CellOption.Create(_.CreatedDay, "dd/MM/yyyy"),
                        _ => CellOption.Create(_.Revenue, "Number"),
                        _ => CellOption.Create(_.Commission, "Number")
                    );
                });
        }

        public FileDto ReportCompareParnerExportToFile(ReportComparePartnerExportInfo input)
        {

            return CreateExcelPackage(
                "BaoCao_DoiSoat_DoiTac.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("TongHop");

                    ICellStyle style = sheet.Workbook.CreateCellStyle();
                    style.BorderTop = BorderStyle.Thin;
                    style.BorderLeft = BorderStyle.Thin;
                    style.BorderRight = BorderStyle.Thin;
                    style.BorderBottom = BorderStyle.Thin;
                    style.VerticalAlignment = VerticalAlignment.Center;
                    style.Alignment = HorizontalAlignment.Center;

                    ICellStyle styleSum = sheet.Workbook.CreateCellStyle();
                    styleSum.BorderTop = BorderStyle.Thin;
                    styleSum.BorderLeft = BorderStyle.Thin;
                    styleSum.BorderRight = BorderStyle.Thin;
                    styleSum.BorderBottom = BorderStyle.Thin;
                    styleSum.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightYellow.Index;
                    styleSum.WrapText = true;
                    styleSum.FillPattern = FillPattern.FineDots;
                    styleSum.VerticalAlignment = VerticalAlignment.Center;
                    styleSum.Alignment = HorizontalAlignment.Center;
                    var font = sheet.Workbook.CreateFont();
                    font.IsBold = true;
                    font.FontHeightInPoints = 12;
                    styleSum.SetFont(font);

                    ICellStyle styleSum2 = sheet.Workbook.CreateCellStyle();
                    styleSum2.SetFont(font);

                    AddObjectsRowItemColumn(sheet, 0, 0, input.Title);
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6));
                    int columsIndex = 0;
                    AddObjectsRowItemColumn(sheet, 1, 0, input.PeriodCompare);
                    sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 6));
                    AddObjectsRowItemColumn(sheet, 2, 0, "ĐỐI TÁC: " + input.Provider);
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 6));
                    AddObjectsRowItemColumn(sheet, 3, 0, input.Contract);

                    sheet.AddMergedRegion(new CellRangeAddress(3, 3, 0, 6));
                    AddObjectsRowItemColumn(sheet, 4, 0, input.PeriodPayment);
                    sheet.AddMergedRegion(new CellRangeAddress(4, 4, 0, 6));
                    int rowsIndex = 6;
                    int rowsType = 1;

                    #region 1.Bảng mã thẻ

                    sheet.SetColumnWidth(0, 7000);
                    sheet.SetColumnWidth(1, 5000);
                    sheet.SetColumnWidth(2, 4000);
                    sheet.SetColumnWidth(3, 4000);
                    sheet.SetColumnWidth(4, 4000);
                    sheet.SetColumnWidth(5, 4000);
                    sheet.SetColumnWidth(6, 6000);

                    if (input.PinCodeItems.Count > 0)
                    {
                        AddObjectsRowItemColumn(sheet, 5, 0, $"{ReportComparePartnerExportInfo.GetIndex(rowsType)}. ĐỐI SOÁT DỊCH VỤ MÃ THẺ", isCenter: false);

                        AddHeaderStartRowIndex(
                           sheet, rowsIndex, columsIndex,
                           "Dịch vụ",
                           "Loại sản phẩm",
                           "Mệnh giá",
                           "Số lượng",
                           "Thành tiền chưa CK",
                           "Tỷ lệ CK",
                           "Tiền thanh toán"
                       );


                        AddObjectsStartRowsIndex(
                            sheet, rowsIndex, columsIndex, style, input.PinCodeItems,
                            _ => _.ServiceName,
                            _ => _.CategoryName,
                            _ => CellOption.Create(_.ProductValue, "Number"),
                            _ => CellOption.Create(_.Quantity, "Number"),
                            _ => CellOption.Create(_.Value, "Number"),
                            _ => CellOption.Create(_.DiscountRate, "Number"),
                            _ => CellOption.Create(_.Price, "Number")
                        );


                        rowsIndex = rowsIndex + input.TotalRowsPinCode + 1;
                        AddObjectsSumRowsIndex(
                           sheet, rowsIndex, columsIndex, styleSum, input.SumPinCodes,
                           _ => "Tổng",
                           _ => "",
                           _ => "",
                           _ => CellOption.Create(_.Quantity, "Number"),
                           _ => CellOption.Create(_.Value, "Number"),
                           _ => "",
                           _ => CellOption.Create(_.Price, "Number")
                       );

                        rowsIndex = rowsIndex + 1;
                        AddObjectsSumRowsIndex(
                          sheet, rowsIndex, columsIndex, styleSum2, input.SumPinCodes,
                          _ => "Tổng doanh số thu được hưởng:",
                          _ => "",
                           _ => CellOption.Create(_.Price, "Number"),
                          _ => "đồng (bao gồm VAT)"
                      );
                        sheet.AddMergedRegion(new CellRangeAddress(rowsIndex, rowsIndex, 0, 1));
                        rowsType = rowsType + 1;

                    }

                    if (input.PinGameItems.Count > 0)
                    {
                        rowsIndex = rowsIndex + 1;
                        AddObjectsRowItemColumn(sheet, rowsIndex, 0, $"{ReportComparePartnerExportInfo.GetIndex(rowsType)}. ĐỐI SOÁT DỊCH VỤ MÃ THẺ GAME", isCenter: false);
                        rowsIndex = rowsIndex + 1;
                        AddHeaderStartRowIndex(
                           sheet, rowsIndex, columsIndex,
                           "Dịch vụ",
                           "Loại sản phẩm",
                           "Mệnh giá",
                           "Số lượng",
                           "Thành tiền chưa CK",
                           "Tỷ lệ CK",
                           "Tiền thanh toán"
                       );


                        AddObjectsStartRowsIndex(
                            sheet, rowsIndex, columsIndex, style, input.PinGameItems,
                            _ => _.ServiceName,
                            _ => _.CategoryName,
                            _ => CellOption.Create(_.ProductValue, "Number"),
                            _ => CellOption.Create(_.Quantity, "Number"),
                            _ => CellOption.Create(_.Value, "Number"),
                            _ => CellOption.Create(_.DiscountRate, "Number"),
                            _ => CellOption.Create(_.Price, "Number")
                        );


                        rowsIndex = rowsIndex + input.TotalRowsPinGame + 1;
                        AddObjectsSumRowsIndex(
                           sheet, rowsIndex, columsIndex, styleSum, input.SumPinGames,
                           _ => "Tổng",
                           _ => "",
                           _ => "",
                           _ => CellOption.Create(_.Quantity, "Number"),
                           _ => CellOption.Create(_.Value, "Number"),
                           _ => "",
                           _ => CellOption.Create(_.Price, "Number")
                       );

                        rowsIndex = rowsIndex + 1;
                        AddObjectsSumRowsIndex(
                          sheet, rowsIndex, columsIndex, styleSum2, input.SumPinGames,
                          _ => "Tổng doanh số thu được hưởng:",
                          _ => "",
                           _ => CellOption.Create(_.Price, "Number"),
                          _ => "đồng (bao gồm VAT)"
                      );
                        sheet.AddMergedRegion(new CellRangeAddress(rowsIndex, rowsIndex, 0, 1));
                        rowsType = rowsType + 1;
                       
                    }

                    #endregion

                    #region 2.Bảng nạp tiền

                    if (input.TopupPrepaIdItems.Count > 0)
                    {
                        rowsIndex = rowsIndex + 1;
                        AddObjectsRowItemColumn(sheet, rowsIndex, 0, $"{ReportComparePartnerExportInfo.GetIndex(rowsType)}. ĐỐI SOÁT DỊCH VỤ NẠP TIỀN TRẢ TRƯỚC", isCenter: false);
                        rowsIndex = rowsIndex + 1;

                        AddHeaderStartRowIndex(
                           sheet, rowsIndex, columsIndex,
                           "Dịch vụ",
                           "Loại sản phẩm",
                           "Mệnh giá",
                           "Số lượng",
                           "Thành tiền chưa CK",
                           "Tỷ lệ CK",
                           "Tiền thanh toán"                         
                       );
                        AddObjectsStartRowsIndex(
                            sheet, rowsIndex, columsIndex, style, input.TopupPrepaIdItems,
                            _ => _.ServiceName,
                            _ => _.CategoryName,
                            _ => CellOption.Create(_.ProductValue, "Number"),
                            _ => CellOption.Create(_.Quantity, "Number"),
                            _ => CellOption.Create(_.Value, "Number"),
                            _ => CellOption.Create(_.DiscountRate, "Number"),
                            _ => CellOption.Create(_.Price, "Number")                          
                        );


                        rowsIndex = rowsIndex + input.TotalRowsTopupPrepaId + 1;

                        AddObjectsSumRowsIndex(
                         sheet, rowsIndex, columsIndex, styleSum, input.SumTopupPrepaId,
                         _ => "Tổng",
                         _ => "",
                         _ => "",
                         _ => CellOption.Create(_.Quantity, "Number"),
                         _ => CellOption.Create(_.Value, "Number"),
                         _ => "",
                         _ => CellOption.Create(_.Price, "Number")
                         );

                        rowsIndex = rowsIndex + 1;
                        AddObjectsSumRowsIndex(
                          sheet, rowsIndex, columsIndex, styleSum2, input.SumTopupPrepaId,
                          _ => "Tổng doanh số thu được hưởng:",
                          _ => "",
                           _ => CellOption.Create(_.Price, "Number"),
                          _ => "đồng (bao gồm VAT)"
                      );

                        sheet.AddMergedRegion(new CellRangeAddress(rowsIndex, rowsIndex, 0, 1));
                        rowsType = rowsType + 1;

                    }

                    if (input.TopupPostpaIdItems.Count > 0)
                    {
                        rowsIndex = rowsIndex + 1;
                        AddObjectsRowItemColumn(sheet, rowsIndex, 0, $"{ReportComparePartnerExportInfo.GetIndex(rowsType)}. ĐỐI SOÁT DỊCH VỤ NẠP TIỀN TRẢ SAU", isCenter: false);
                        rowsIndex = rowsIndex + 1;

                        AddHeaderStartRowIndex(
                           sheet, rowsIndex, columsIndex,
                           "Dịch vụ",
                           "Loại sản phẩm",
                           "Mệnh giá",
                           "Số lượng",
                           "Thành tiền chưa CK",
                           "Tỷ lệ CK",
                           "Tiền thanh toán"                         
                       );
                        AddObjectsStartRowsIndex(
                            sheet, rowsIndex, columsIndex, style, input.TopupPostpaIdItems,
                            _ => _.ServiceName,
                            _ => _.CategoryName,
                            _ => CellOption.Create(_.ProductValue, "Number"),
                            _ => CellOption.Create(_.Quantity, "Number"),
                            _ => CellOption.Create(_.Value, "Number"),
                            _ => CellOption.Create(_.DiscountRate, "Number"),
                            _ => CellOption.Create(_.Price, "Number")                           
                        );


                        rowsIndex = rowsIndex + input.TotalRowsTopupPostpaId + 1;

                        AddObjectsSumRowsIndex(
                         sheet, rowsIndex, columsIndex, styleSum, input.SumTopupPostpaId,
                         _ => "Tổng",
                         _ => "",
                         _ => "",
                         _ => CellOption.Create(_.Quantity, "Number"),
                         _ => CellOption.Create(_.Value, "Number"),
                         _ => "",
                         _ => CellOption.Create(_.Price, "Number")
                         );

                        rowsIndex = rowsIndex + 1;
                        AddObjectsSumRowsIndex(
                          sheet, rowsIndex, columsIndex, styleSum2, input.SumTopupPostpaId,
                          _ => "Tổng doanh số thu được hưởng:",
                          _ => "",
                           _ => CellOption.Create(_.Price, "Number"),
                          _ => "đồng (bao gồm VAT)"
                      );

                        sheet.AddMergedRegion(new CellRangeAddress(rowsIndex, rowsIndex, 0, 1));
                        rowsType = rowsType + 1;

                    }

                    if (input.TopupItems.Count > 0)
                    {
                        rowsIndex = rowsIndex + 1;
                        AddObjectsRowItemColumn(sheet, rowsIndex, 0, $"{ReportComparePartnerExportInfo.GetIndex(rowsType)}. ĐỐI SOÁT DỊCH VỤ NẠP TIỀN", isCenter: false);
                        rowsIndex = rowsIndex + 1;

                        AddHeaderStartRowIndex(
                           sheet, rowsIndex, columsIndex,
                           "Dịch vụ",
                           "Loại sản phẩm",
                           "Mệnh giá",
                           "Số lượng",
                           "Thành tiền chưa CK",
                           "Tỷ lệ CK",
                           "Tiền thanh toán"                        
                       );
                        AddObjectsStartRowsIndex(
                            sheet, rowsIndex, columsIndex, style, input.TopupItems,
                            _ => _.ServiceName,
                            _ => _.CategoryName,
                            _ => CellOption.Create(_.ProductValue, "Number"),
                            _ => CellOption.Create(_.Quantity, "Number"),
                            _ => CellOption.Create(_.Value, "Number"),
                            _ => CellOption.Create(_.DiscountRate, "Number"),
                            _ => CellOption.Create(_.Price, "Number")                            
                        );


                        rowsIndex = rowsIndex + input.TotalRowsTopup + 1;

                        AddObjectsSumRowsIndex(
                         sheet, rowsIndex, columsIndex, styleSum, input.SumTopup,
                         _ => "Tổng",
                         _ => "",
                         _ => "",
                         _ => CellOption.Create(_.Quantity, "Number"),
                         _ => CellOption.Create(_.Value, "Number"),
                         _ => "",
                         _ => CellOption.Create(_.Price, "Number")
                         );

                        rowsIndex = rowsIndex + 1;
                        AddObjectsSumRowsIndex(
                          sheet, rowsIndex, columsIndex, styleSum2, input.SumTopup,
                          _ => "Tổng doanh số thu được hưởng:",
                          _ => "",
                           _ => CellOption.Create(_.Price, "Number"),
                          _ => "đồng (bao gồm VAT)"
                      );

                        sheet.AddMergedRegion(new CellRangeAddress(rowsIndex, rowsIndex, 0, 1));
                        rowsType = rowsType + 1;

                    }

                    #endregion

                    #region 3.Data

                    if (input.DataItems.Count > 0)
                    {
                        rowsIndex = rowsIndex + 1;
                        AddObjectsRowItemColumn(sheet, rowsIndex, 0, $"{ReportComparePartnerExportInfo.GetIndex(rowsType)}. ĐỐI SOÁT DỊCH VỤ DATA", isCenter: false);
                        rowsIndex = rowsIndex + 1;

                        AddHeaderStartRowIndex(
                           sheet, rowsIndex, columsIndex,
                           "Dịch vụ",
                           "Loại sản phẩm",
                           "Mệnh giá",
                           "Số lượng",
                           "Thành tiền chưa CK",
                           "Tỷ lệ CK",
                           "Tiền thanh toán"                         
                       );
                        AddObjectsStartRowsIndex(
                            sheet, rowsIndex, columsIndex, style, input.DataItems,
                            _ => _.ServiceName,
                            _ => _.CategoryName,
                            _ => CellOption.Create(_.ProductValue, "Number"),
                            _ => CellOption.Create(_.Quantity, "Number"),
                            _ => CellOption.Create(_.Value, "Number"),
                            _ => CellOption.Create(_.DiscountRate, "Number"),
                            _ => CellOption.Create(_.Price, "Number")                           
                        );


                        rowsIndex = rowsIndex + input.TotalRowsData + 1;

                        AddObjectsSumRowsIndex(
                         sheet, rowsIndex, columsIndex, styleSum, input.SumData,
                         _ => "Tổng",
                         _ => "",
                         _ => "",
                         _ => CellOption.Create(_.Quantity, "Number"),
                         _ => CellOption.Create(_.Value, "Number"),
                         _ => "",
                         _ => CellOption.Create(_.Price, "Number")
                         );

                        rowsIndex = rowsIndex + 1;
                        AddObjectsSumRowsIndex(
                          sheet, rowsIndex, columsIndex, styleSum2, input.SumData,
                          _ => "Tổng doanh số thu được hưởng:",
                          _ => "",
                           _ => CellOption.Create(_.Price, "Number"),
                          _ => "đồng (bao gồm VAT)"
                      );

                        sheet.AddMergedRegion(new CellRangeAddress(rowsIndex, rowsIndex, 0, 1));
                        rowsType = rowsType + 1;

                    }

                    #endregion

                    #region 4.Thanh toán hóa đơn

                    if (input.PayBillItems.Count > 0)
                    {
                        rowsIndex = rowsIndex + 1;
                        AddObjectsRowItemColumn(sheet, rowsIndex, 0, $"{ReportComparePartnerExportInfo.GetIndex(rowsType)}. ĐỐI SOÁT DỊCH VỤ THANH TOÁN HOÁ ĐƠN", isCenter: false);
                        rowsIndex = rowsIndex + 1;

                        AddHeaderStartRowIndex(
                           sheet, rowsIndex, columsIndex,
                           "Loại sản phẩm",
                           "Sản phẩm",
                           "Số lượng GD",
                           "Giá trị GD (chưa phí)",
                           "Phí GD",
                           "Tổng tiền phí GD",
                           "Tiền phí được hưởng"
                       );

                        AddObjectsStartRowsIndex(
                            sheet, rowsIndex, columsIndex, style, input.PayBillItems,
                            _ => _.CategoryName,
                            _ => _.ProductName,
                            _ => CellOption.Create(_.Quantity, "Number"),
                            _ => CellOption.Create(_.Value, "Number"),
                            _ => _.FeeText,
                            _ => CellOption.Create(_.Fee, "Number"),
                            _ => CellOption.Create(_.Discount, "Number")
                        );


                        rowsIndex = rowsIndex + input.TotalRowsPayBill + 1;

                        AddObjectsSumRowsIndex(
                         sheet, rowsIndex, columsIndex, styleSum, input.SumPayBill,
                         _ => "Tổng",
                         _ => "",
                         _ => CellOption.Create(_.Quantity, "Number"),
                         _ => CellOption.Create(_.Value, "Number"),
                         _ => "",
                         _ => CellOption.Create(_.Fee, "Number"),
                         _ => CellOption.Create(_.Discount, "Number")
                         );

                        rowsIndex = rowsIndex + 1;
                        AddObjectsSumRowsIndex(
                          sheet, rowsIndex, columsIndex, styleSum2, input.SumPayBill,
                          _ => $"Số tiền phí {input.Provider} được hưởng:",
                          _ => "",
                           _ => CellOption.Create(input.SumPayBill.Discount, "Number"),
                          _ => "đồng (bao gồm VAT)"
                      );

                        sheet.AddMergedRegion(new CellRangeAddress(rowsIndex, rowsIndex, 0, 1));
                        rowsIndex = rowsIndex + 1;
                        AddObjectsRowItemColumn(sheet, rowsIndex, 0, "Số tiền bằng chữ:", isCenter: false);
                        rowsIndex = rowsIndex + 1;
                        AddObjectsRowItemColumn(sheet, rowsIndex, 0, "(Số tiền đã bao gồm thuế VAT)", isCenter: false);
                        rowsIndex = rowsIndex + 1;
                        AddObjectsRowItemColumn(sheet, rowsIndex, 0, "Trong đó", isCenter: false);
                        rowsIndex = rowsIndex + 1;

                        var discountVat = Math.Round(input.SumPayBill.Discount / 11, 0);
                        AddObjectsSumRowsIndex(
                         sheet, rowsIndex, columsIndex + 1, null, input.SumPayBill,
                         _ => $"Giá trị trước thuế :",
                          _ => CellOption.Create(input.SumPayBill.Discount - discountVat, "Number"));

                        rowsIndex = rowsIndex + 1;
                        AddObjectsSumRowsIndex(
                        sheet, rowsIndex, columsIndex + 1, null, input.SumPayBill,
                        _ => $"Thuế GTGT 10% :",
                         _ => CellOption.Create(discountVat, "Number"));

                        rowsType = rowsType + 1;
                    }

                    #endregion

                    #region 5.Công nợ

                    rowsIndex = rowsIndex + 1;
                    AddObjectsRowItemColumn(sheet, rowsIndex, 0, $"{ReportComparePartnerExportInfo.GetIndex(rowsType)}. CÔNG NỢ", isCenter: false);
                    rowsIndex = rowsIndex + 1;
                    AddHeaderStartRowIndex(
                          sheet, rowsIndex, columsIndex,
                          "STT",
                          "Nội dung",
                          "Thành tiền"
                      );
                    AddObjectsStartRowsIndex(
                           sheet, rowsIndex, columsIndex, style, input.BalanceItems,
                              _ => _.Index,
                              _ => _.Name,
                              _ => CellOption.Create(_.Value, "Number")
                        );

                    rowsIndex = rowsIndex + input.TotalRowsBalance + 1;
                    AddObjectsRowItemColumn(sheet, rowsIndex, 4, "Ngày.......tháng.......năm.........");
                    rowsIndex = rowsIndex + 1;
                    AddObjectsRowItemColumn(sheet, rowsIndex, 1, "CÔNG TY TM DV NHẤT TRẦN");
                    AddObjectsRowItemColumn(sheet, rowsIndex, 4, input.FullName, false);

                    #endregion
                });
        }

        public FileDto ReportTopupRequestLogExportToFile(List<ReportTopupRequestLogDto> input)
        {
            return CreateExcelPackage(
                "Bao cao chi tiet lich su giao dich.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Detail");
                    AddHeader(
                        sheet,
                        "Mã giao dịch",
                    "Mã giao dịch NCC",
                    "Dịch vụ",
                    "Loại sản phẩm",
                    "Mã sản phẩm",
                    "Nhà cung cấp",
                    "Mã đối tác",
                    "Thành tiền",
                    "Số thụ hưởng",
                    "Mã giao dịch đối soát",
                    "Thời gian bắt đầu",
                    "Thời gian kết thúc",
                    "Trạng thái",
                    "Dữ liệu trả về từ NCC"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.TransRef,
                    _ => _.TransCode,
                    _ => _.ServiceCode,
                    _ => _.CategoryCode,
                    _ => _.ProductCode,
                    _ => _.ProviderCode,
                    _ => _.PartnerCode,
                    _ => CellOption.Create(_.TransAmount, "Number"),
                    _ => _.ReceiverInfo,
                    _ => _.TransIndex,
                    _ => CellOption.Create(_.RequestDate, "dd/MM/yyyy HH:mm:ss"),
                    _ => CellOption.Create(_.ModifiedDate, "dd/MM/yyyy HH:mm:ss"),
                    _ => _.StatusName,
                    _ => _.ResponseInfo
                    );
                });
        }
    }
}
