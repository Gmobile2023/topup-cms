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

namespace HLS.Topup.Reports.Exporting
{
    public partial class ReportExcelExporter
    {
        public FileDto ReportCommissionDetailExportToFile(List<ReportCommissionDetailDto> input)
        {
            string fileName = string.Format("Bao cao chi tiet hoa hong dai ly tong.xlsx");
            return CreateExcelPackage(
                fileName,
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Sheet1");
                    AddHeader(
                        sheet,
                        "Đại lý tổng",
                        "Mã GD trả hoa hồng",
                        "Hoa hồng",
                        "Tình trạng",
                        "Thời gian trả",
                        "Đại lý cấp 1",
                        "Mã GD",
                        "Mã đối tác",
                        "Dịch vụ",
                        "Loại sản phẩm",
                        "Sản phẩm",
                        "Thời gian"
                    );


                    AddObjects(
                        sheet, 2, input,
                        _ => _.AgentSumCode,
                        _ => _.CommissionCode,
                        _ => _.CommissionAmount,
                        _ => _.StatusName,
                        _ => CellOption.Create(_.PayDate, "dd/MM/yyyy HH:mm:ss"),
                        _ => _.AgentCode,
                        _ => _.TransCode,
                        _ => _.RequestRef,
                        _ => _.ServiceName,
                        _ => _.CategoryName,
                        _ => _.ProductName,
                        _ => CellOption.Create(_.CreateDate, "dd/MM/yyyy HH:mm:ss")                        
                    );
                });
        }


        public FileDto ReportCommissionTotalExportToFile(List<ReportCommissionTotalDto> input)
        {
            string fileName = string.Format("Bao cao tong hop hoa hong dai ly tong.xlsx");
            return CreateExcelPackage(
                fileName,
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Sheet1");
                    AddHeader(
                        sheet,
                        "Mã đại lý tổng",
                        "Tên đại lý tổng",
                        "Số lượng giao dịch",
                        "Hoa hồng",
                        "Đã trả",
                        "Chưa trả"
                    );


                    AddObjects(
                        sheet, 2, input,
                        _ => _.AgentCode,
                        _ => _.AgentName,
                        _ => CellOption.Create(_.Quantity, "Number"),
                        _ => CellOption.Create(_.CommissionAmount, "Number"),
                        _ => CellOption.Create(_.Payment, "Number"),
                        _ => CellOption.Create(_.UnPayment, "Number")
                    );
                });
        }


        public FileDto ReportCommissionAgentDetailExportToFile(List<ReportCommissionAgentDetailDto> input)
        {
            string fileName = string.Format("Bao cao chi tiet ban hang dai ly cap 1.xlsx");
            return CreateExcelPackage(
                fileName,
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Sheet1");
                    AddHeader(
                        sheet,
                        "Đại lý cấp 1",
                        "Mã GD trả hoa hồng",
                        "Hoa hồng",
                        "Tình trạng",
                        "Thời gian trả hoa hồng",
                        "Mã GD",
                        "Dịch vụ",
                        "Loại sản phẩm",
                        "Sản phẩm",
                        "Đơn giá",
                        "Số lượng",
                        "Chiết khấu",
                        "Phí",
                        "Thành tiền",
                        "Trạng thái GD",
                        "Thời gian giao dịch"
                    );


                    AddObjects(
                        sheet, 2, input,
                        _ => _.AgentCode,
                        _ => _.CommissionCode,
                        _ => _.CommissionAmount,
                        _ => _.StatusPaymentName,
                        _ => CellOption.Create(_.PayDate, "dd/MM/yyyy HH:mm:ss"),
                        _ => _.RequestRef,
                        _ => _.ServiceName,
                        _ => _.CategoryName,
                        _ => _.ProductName,
                        _ => _.Price,
                        _ => _.Quantity,
                        _ => _.Discount,
                        _ => _.Fee,
                        _ => _.TotalPrice,
                        _ => _.StatusName,
                        _ => CellOption.Create(_.CreateDate, "dd/MM/yyyy HH:mm:ss")
                    );
                });
        }


        public FileDto ReportCommissionAgentTotalExportToFile(List<ReportCommissionAgentTotalDto> input)
        {
            string fileName = string.Format("Bao cao tong hop dai ly cap 1.xlsx");
            return CreateExcelPackage(
                fileName,
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Sheet1");
                    AddHeader(
                        sheet,
                        "Mã",
                        "Tên",
                        "Số dư đầu kỳ",
                        "Phát sinh tăng",
                        "Phát sinh giảm",
                        "Số dư cuối kỳ"
                    );


                    AddObjects(
                        sheet, 2, input,
                        _ => _.AgentCode,
                        _ => _.AgentName,
                        _ => CellOption.Create(_.Before, "Number"),
                        _ => CellOption.Create(_.AmountUp, "Number"),
                        _ => CellOption.Create(_.AmountDown, "Number"),
                        _ => CellOption.Create(_.After, "Number")
                    );
                });
        }

    }
}
