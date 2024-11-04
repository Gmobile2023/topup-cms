using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Providers.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;
using HLS.Topup.Compare;

namespace HLS.Topup.Providers.Exporting
{
    public class ProvidersExcelExporter : NpoiExcelExporterBase, IProvidersExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProvidersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProviderForViewDto> providers)
        {
            return CreateExcelPackage(
                "Providers.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Providers"));

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name"),
                        L("PhoneNumber"),
                        L("ProviderType"),
                        L("ProviderStatus")
                    );

                    AddObjects(
                        sheet, 2, providers,
                        _ => _.Provider.Code,
                        _ => _.Provider.Name,
                        _ => _.Provider.PhoneNumber,
                        _ => _.Provider.ProviderType,
                        _ => _.Provider.ProviderStatus
                    );
                });
        }

        public FileDto ExportCompareToFile(List<CompareDtoReponse> input)
        {
            return CreateExcelPackage(
                "Đối soát.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Report");
                    AddHeader(
                        sheet,
                        "Ngày đối soát",
                        "Ngày giao dịch",
                        "Nhà cung cấp",
                        "Tên BF NT",
                        "Tên BF NCC",
                        "Tổng SL BF NT",
                        "Tổng Mệnh giá BF NT",
                        "Tổng SL BF NCC",
                        "Tổng Mệnh giá BF NCC",
                        "SL khớp",
                        "SL lệch",
                        "NT có NCC không có",
                        "NCC có NT không có",
                        "Người đối soát"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.CompareDate,
                        _ => _.TransDate,
                        _ => _.ProviderCode,
                        _ => _.SysFileName,
                        _ => _.ProviderFileName,
                        _ => CellOption.Create(_.SysQuantity, "Number"),
                        _ => CellOption.Create(_.SysAmount, "Number"),
                        _ => CellOption.Create(_.ProviderQuantity, "Number"),
                        _ => CellOption.Create(_.ProviderAmount, "Number"),
                        _ => CellOption.Create(_.SameQuantity, "Number"),
                        _ => CellOption.Create(_.NotSameAmount, "Number"),
                        _ => CellOption.Create(_.SysOnlyQuantity, "Number"),
                        _ => CellOption.Create(_.ProviderOnlyQuantity, "Number"),
                        _ => _.AccountCompare                        
                    );
                });
        }

        public FileDto ExportCompareDetailToFile(List<CompareReponseDetailDto> input)
        {
            return CreateExcelPackage(
                "Đối soát chi tiết.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Report");
                    AddHeader(
                        sheet,
                        "Thời gian GD",
                        "Đại lý",
                        "Mã giao dịch",
                        "Mã NCC",
                        "Mệnh giá",
                        "Số thụ hưởng",
                        "Loại sản phẩm"                   
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.TransDate,
                        _ => _.AgentCode,
                        _ => _.TransCode,
                        _ => _.TransPay,
                        _ => CellOption.Create(_.ProductValue, "Number"),
                        _ => _.ReceivedAccount,
                        _ => _.ProductCode
                    );
                });
        }

        public FileDto ExportCompareRefundToFile(List<CompareRefunDto> input)
        {
            return CreateExcelPackage(
                "Hoàn tiền.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Report");
                    AddHeader(
                        sheet,                        
                        "Ngày giao dịch",
                        "Nhà cung cấp",
                        "Tổng số lượng",
                        "Tổng số tiền",
                        "SL đã hoàn",
                        "Số tiền đã hoàn",
                        "Số lượng chưa hoàn",
                        "Số tiền chưa hoàn"                       
                    );

                    AddObjects(
                        sheet, 2, input,                        
                        _ => _.TransDate,
                        _ => _.Provider,                        
                        _ => CellOption.Create(_.Quantity, "Number"),                        
                        _ => CellOption.Create(_.Amount, "Number"),
                        _ => CellOption.Create(_.RefundQuantity, "Number"),
                        _ => CellOption.Create(_.RefundAmount, "Number"),
                        _ => CellOption.Create(_.RefundWaitQuantity, "Number"),
                        _ => CellOption.Create(_.RefundWaitAmount, "Number")
                    );
                });
        }

        public FileDto ExportCompareRefundDetailToFile(List<CompareRefunDetailDto> input)
        {
            return CreateExcelPackage(
                "Chi tiết hoàn tiền.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Report");
                    AddHeader(
                        sheet,
                        "Thời gian GD",
                        "Đại lý",
                        "Mã giao dịch",
                        "Mã NCC",
                        "Mệnh giá",
                        "Thành tiền",
                        "Số thụ hưởng",
                        "Loại sản phẩm",
                        "Tình trạng",
                        "Mã GD hoàn tiền"
                    );

                    AddObjects(
                        sheet, 2, input,
                        _ => _.TransDate,
                        _ => _.AgentCode,
                        _ => _.TransCode,
                        _ => _.TransPay,
                        _ => CellOption.Create(_.ProductValue, "Number"),
                        _ => CellOption.Create(_.Amount, "Number"),
                        _ => _.ReceivedAccount,
                        _ => _.ProductCode,
                        _ => _.StatusName,
                        _ => _.TransCodeRefund                      
                    );
                });
        }
    }
}