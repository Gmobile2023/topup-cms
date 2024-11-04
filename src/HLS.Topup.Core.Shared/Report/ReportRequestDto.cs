using ServiceStack;
using System;
using HLS.Topup.Common;
using System.Collections.Generic;

namespace HLS.Topup.Report
{
    [Route("/api/v1/report/ReportDetail", "GET")]
    public class ReportDetailRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string AccountCode { get; set; }
        public string TransCode { get; set; }
        public string Filter { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string LoginCode { get; set; }
        public int AccountType { get; set; }
    }

    [Route("/api/v1/report/balanceTotal", "GET")]
    public class BalanceTotalRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string AccountCode { get; set; }
        public int AgentType { get; set; }

        public string LoginCode { get; set; }
        public int AccountType { get; set; }
    }

    [Route("/api/v1/report/BalanceGroupTotal", "GET")]
    public class BalanceGroupTotalRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }


    [Route("/api/v1/report/cardstock/card_stock_histories", "GET")]
    public class ReportCardStockHistoriesRequest : PaggingBaseDto
    {
        public int CardValue { get; set; }
        public string Telco { get; set; }
        public string StockCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string StockType { get; set; }
        public string ProductCode { get; set; }
    }


    [Route("/api/v1/report/cardstock/card_stock_ImExPort", "GET")]
    public class ReportCardStockImExPortRequest : PaggingBaseDto
    {
        public string StoreCode { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }



    [Route("/api/v1/report/cardstock/card_stock_imexportprovider", "GET")]
    public class ReportCardStockImExProviderRequest : PaggingBaseDto
    {
        public string StoreCode { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public string ProviderCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    [Route("/api/v1/report/cardstock/card_stock_Auto", "GET")]
    public class ReportCardStockAutoRequest : PaggingBaseDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    [Route("/api/v1/report/cardstock/card_stock_inventory", "GET")]
    public class ReportCardStockInventoryRequest : PaggingBaseDto
    {
        public int CardValue { get; set; }
        public string Telco { get; set; }
        public string StockCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string StockType { get; set; }
        public string ProductCode { get; set; }
    }

    [Route("/api/v1/report/ReportTransDetail", "GET")]
    public class ReporttransDetailRequest : PaggingBaseDto
    {
        public string Filter { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string RequestTransCode { get; set; }
        public string ReceivedAccount { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProviderCode { get; set; }
        public string UserProcess { get; set; }
        public int Status { get; set; }
        public string AccountCode { get; set; }

        public int Type { get; set; }

        public string ExportType { get; set; }
    }

    [Route("/api/v1/report/TransDetailByTransCode", "GET")]
    public class TransDetailByTransCodeRequest
    {
        public string TransCode { get; set; }

        public string Type { get; set; }
    }

    [Route("/api/v1/report/ReportTotalDay", "GET")]
    public class ReportTotalDayRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string AccountCode { get; set; }
    }

    [Route("/api/v1/report/ReportDebtDetail", "GET")]
    public class ReportDebtDetailRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string TransCode { get; set; }
        public string ServiceCode { get; set; }
        public string AccountCode { get; set; }
        public string LoginCode { get; set; }
        public int AccountType { get; set; }
    }

    [Route("/api/v1/report/ReportRefundDetail", "GET")]
    public class ReportRefundDetailRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Đại lý
        /// </summary>
        public string AgentCode { get; set; }

        /// <summary>
        /// Mã giao dịch
        /// </summary>
        public string TransCode { get; set; }


        /// <summary>
        /// Mã giao dịch gốc
        /// </summary>
        public string TransCodeSouce { get; set; }

        /// <summary>
        /// Dịch vụ
        /// </summary>
        public List<string> ServiceCode { get; set; }

        /// <summary>
        /// Loại sản phẩm
        /// </summary>
        public List<string> CategoryCode { get; set; }

        /// <summary>
        /// Sản phẩm
        /// </summary>
        public List<string> ProductCode { get; set; }

        public string LoginCode { get; set; }
        public int AccountType { get; set; }      
    }


    [Route("/api/v1/report/ReportTransferDetail", "GET")]
    public class ReportTransferDetailRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Loại đại lý
        /// </summary>
        public int AgentType { get; set; }

        /// <summary>
        /// Đại lý/Chuyển-Nhận
        /// </summary>
        public string AgentTransferCode { get; set; }

        public string AgentReceiveCode { get; set; }

        /// <summary>
        /// Dịch vụ
        /// </summary>
        public string ServiceCode { get; set; }

        public string LoginCode { get; set; }
        public int AccountType { get; set; }
    }

    [Route("/api/v1/report/ReportServiceDetail", "GET")]
    public class ReportServiceDetailRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Loại đại lý
        /// </summary>
        public int AgentType { get; set; }

        /// <summary>
        /// Đại lý
        /// </summary>
        public string AgentCode { get; set; }

        /// <summary>
        /// Đại lý tổng
        /// </summary>
        public string AgentCodeParent { get; set; }
        /// <summary>
        /// Người thực hiện
        /// </summary>
        public string UserAgentStaffCode { get; set; }

        /// <summary>
        /// Số thụ hưởng
        /// </summary>
        public string ReceivedAccount { get; set; }

        /// <summary>
        /// Nhân viên sale
        /// </summary>
        public string UserSaleCode { get; set; }

        /// <summary>
        /// Sale Leader
        /// </summary>
        public string UserSaleLeaderCode { get; set; }

        // <summary>
        /// Mã giao dịch NT sinh ra
        /// </summary>
        public string TransCode { get; set; }

        /// <summary>
        /// Mã giao dịch đối tác gọi sang NT
        /// </summary>
        public string RequestRef { get; set; }


        /// <summary>
        /// Mã giao dịch NT gọi sang NCC
        /// </summary>
        public string PayTransRef { get; set; }


        /// <summary>
        /// Nhà cung cấp
        /// </summary>
        public List<string> VenderCode { get; set; }

        /// <summary>
        /// Nhà cung cấp cha
        /// </summary>
        public string ParentProvider { get; set; }

        /// <summary>
        /// Dịch vụ
        /// </summary>
        public List<string> ServiceCode { get; set; }

        /// <summary>
        /// Loại sản phẩm
        /// </summary>
        public List<string> CategoryCode { get; set; }

        /// <summary>
        /// Sản phẩm
        /// </summary>
        public List<string> ProductCode { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }


        /// <summary>
        /// Tỉnh/TP
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Quận huyện
        /// </summary>
        public int DistrictId { get; set; }

        /// <summary>
        /// Phường/xã
        /// </summary>
        public int WardId { get; set; }

        public string LoginCode { get; set; }

        public int AccountType { get; set; }

        public string ReceiverType { get; set; }
        public string ProviderTransCode { get; set; }
        public string ProviderReceiverType { get; set; }
    }


    [Route("/api/v1/report/ReportServiceTotal", "GET")]
    public class ReportServiceTotalRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Dịch vụ
        /// </summary>
        public List<string> ServiceCode { get; set; }

        /// <summary>
        /// Loại sản phẩm
        /// </summary>
        public List<string> CategoryCode { get; set; }

        /// <summary>
        /// Sản phẩm
        /// </summary>
        public List<string> ProductCode { get; set; }

        public string LoginCode { get; set; }

        public int AccountType { get; set; }

        /// <summary>
        /// Loại đại lý
        /// </summary>
        public int AgentType { get; set; }

        /// <summary>
        /// Đại lý
        /// </summary>
        public string AgentCode { get; set; }
        public string ReceiverType { get; set; }
        public string ProviderReceiverType { get; set; }
    }

    [Route("/api/v1/report/ReportServiceProvider", "GET")]
    public class ReportServiceProviderRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Loại đại lý
        /// </summary>
        public int AgentType { get; set; }

        /// <summary>
        /// Đại lý
        /// </summary>
        public string AgentCode { get; set; }

        /// <summary>
        /// Nhà cung cấp
        /// </summary>
        public List<string> ProviderCode { get; set; }
        /// <summary>
        /// Dịch vụ
        /// </summary>
        public List<string> ServiceCode { get; set; }

        /// <summary>
        /// Loại sản phẩm
        /// </summary>
        public List<string> CategoryCode { get; set; }

        /// <summary>
        /// Sản phẩm
        /// </summary>
        public List<string> ProductCode { get; set; }
        public string ReceiverType { get; set; }
        public string ProviderReceiverType { get; set; }
        public string LoginCode { get; set; }
        public int AccountType { get; set; }
    }


    [Route("/api/v1/report/ReportAgentBalance", "GET")]
    public class ReportAgentBalanceRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Loại đại lý
        /// </summary>
        public int AgentType { get; set; }

        /// <summary>
        /// Đại lý
        /// </summary>
        public string AgentCode { get; set; }

        /// <summary>
        /// Nhân viên sale
        /// </summary>
        public string UserSaleCode { get; set; }

        /// <summary>
        /// Sale Leader
        /// </summary>
        public string UserSaleLeaderCode { get; set; }

        public string LoginCode { get; set; }
        public int AccountType { get; set; }
    }

    [Route("/api/v1/report/ReportRevenueAgent", "GET")]
    public class ReportRevenueAgentRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Loại đại lý
        /// </summary>
        public int AgentType { get; set; }

        /// <summary>
        /// Đại lý
        /// </summary>
        public string AgentCode { get; set; }

        /// <summary>
        /// Nhân viên quản lý
        /// </summary>
        public string UserSaleCode { get; set; }

        /// <summary>
        /// Sale Leader
        /// </summary>
        public string UserSaleLeaderCode { get; set; }

        /// <summary>
        /// Dịch vụ
        /// </summary>
        public List<string> ServiceCode { get; set; }

        /// <summary>
        /// Loại sản phẩm
        /// </summary>
        public List<string> CategoryCode { get; set; }

        /// <summary>
        /// Sản phẩm
        /// </summary>
        public List<string> ProductCode { get; set; }

        /// <summary>
        /// Tỉnh/TP
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Quận huyện
        /// </summary>
        public int DistrictId { get; set; }

        /// <summary>
        /// Phường/xã
        /// </summary>
        public int WardId { get; set; }

        public string LoginCode { get; set; }
        public int AccountType { get; set; }
    }

    [Route("/api/v1/report/ReportRevenueCity", "GET")]
    public class ReportRevenueCityRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Loại đại lý
        /// </summary>
        public int AgentType { get; set; }

        /// <summary>
        /// Nhân viên quản lý
        /// </summary>
        public string UserSaleCode { get; set; }

        /// <summary>
        /// Sale Leader
        /// </summary>
        public string UserSaleLeaderCode { get; set; }

        /// <summary>
        /// Tỉnh/TP
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Quận huyện
        /// </summary>
        public int DistrictId { get; set; }

        /// <summary>
        /// Phường/xã
        /// </summary>
        public int WardId { get; set; }

        /// <summary>
        /// Dịch vụ
        /// </summary>
        public List<string> ServiceCode { get; set; }

        /// <summary>
        /// Loại sản phẩm
        /// </summary>
        public List<string> CategoryCode { get; set; }

        /// <summary>
        /// Sản phẩm
        /// </summary>
        public List<string> ProductCode { get; set; }

        public string LoginCode { get; set; }
        public int AccountType { get; set; }
    }

    [Route("/api/v1/report/ReportTotalSaleAgent", "GET")]
    public class ReportTotalSaleAgentRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Loại đại lý
        /// </summary>
        public int AgentType { get; set; }

        /// <summary>
        /// Đại lý
        /// </summary>
        public string AgentCode { get; set; }

        /// <summary>
        /// Nhân viên quản lý
        /// </summary>
        public string UserSaleCode { get; set; }

        /// <summary>
        /// Sale Leader
        /// </summary>
        public string UserSaleLeaderCode { get; set; }

        /// <summary>
        /// Tỉnh/TP
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Quận huyện
        /// </summary>
        public int DistrictId { get; set; }

        /// <summary>
        /// Phường/xã
        /// </summary>
        public int WardId { get; set; }

        public List<string> ServiceCode { get; set; }

        /// <summary>
        /// Loại sản phẩm
        /// </summary>
        public List<string> CategoryCode { get; set; }

        /// <summary>
        /// Sản phẩm
        /// </summary>
        public List<string> ProductCode { get; set; }

        public string LoginCode { get; set; }
        public int AccountType { get; set; }
    }

    [Route("/api/v1/report/ReportRevenueActive", "GET")]
    public class ReportRevenueActiveRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Loại đại lý
        /// </summary>
        public int AgentType { get; set; }

        /// <summary>
        /// Đại lý
        /// </summary>
        public string AgentCode { get; set; }

        /// <summary>
        /// Nhân viên quản lý
        /// </summary>
        public string UserSaleCode { get; set; }

        /// <summary>
        /// Sale Leader
        /// </summary>
        public string UserSaleLeaderCode { get; set; }

        /// <summary>
        /// Tỉnh/TP
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Quận huyện
        /// </summary>
        public int DistrictId { get; set; }

        /// <summary>
        /// Phường/xã
        /// </summary>
        public int WardId { get; set; }

        /// <summary>
        /// Trang thái
        /// </summary>
        public int Status { get; set; }

        public string LoginCode { get; set; }
        public int AccountType { get; set; }
    }

    [Route("/api/v1/report/ReportRevenueDashBoardDay", "GET")]
    public class ReportRevenueDashBoardDayRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string LoginCode { get; set; }
        public int AccountType { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
    }

    [Route("/api/v1/report/ReportAgentGeneralDash", "GET")]
    public class ReportAgentGeneralDashRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int AccountType { get; set; }
        public string LoginCode { get; set; }
        public List<string> ServiceCode { get; set; }
        public List<string> CategoryCode { get; set; }
        public List<string> ProductCode { get; set; }
        public string AgentCode { get; set; }
    }

    [Route("/api/v1/report/ReportTotalDebt", "GET")]
    public class ReportTotalDebtRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string AccountCode { get; set; }

        public string LoginCode { get; set; }
        public int AccountType { get; set; }
    }

    [Route("/api/v1/report/RevenueInDay", "GET")]
    public class GetRevenueInDayRequest
    {
        public string AccountCode { get; set; }
    }

    [Route("/api/v1/report/SyncAccountRequest", "POST")]
    public class SyncAccountRequest
    {
        public long UserId { get; set; }
        public string AccountCode { get; set; }
    }

    [Route("/api/v1/report/sms/add", "POST")]
    public class SmsMessageRequest
    {
        public int? TenantId { get; set; }
        public string Description { get; set; }
        public string AccountCode { get; set; }
        public string PhoneNumber { get; set; }
        public string SmsChannel { get; set; }
        public CommonConst.Channel Channel { get; set; }
        public string Result { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public string TransCode { get; set; }
    }

    [Route("/api/v1/report/ReportComparePartner", "GET")]
    public class ReportComparePartnerRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Đại ly
        /// </summary>
        public string AgentCode { get; set; }

        public string Type { get; set; }

        public string ChangerType { get; set; }

        public string ServiceCode { get; set; }
    }


    [Route("/api/v1/report/SendMailCompare", "POST")]
    public class SendMailComparePartnerRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        /// <summary>
        /// Đại ly
        /// </summary>
        public string AgentCode { get; set; }

        public string Email { get; set; }
    }


    [Route("/api/v1/report/ReportCommissionDetail", "GET")]
    public class ReportCommissionDetailRequest : PaggingBaseDto
    {
        /// <summary>
        /// Tìm kiếm chung
        /// </summary>
        public string Filter { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string TransCode { get; set; }
        /// <summary>
        /// Đại lý tổng
        /// </summary>
        public string AgentCodeSum { get; set; }
        /// <summary>
        /// Đại lý
        /// </summary>
        public string AgentCode { get; set; }     

        /// <summary>
        /// Dịch vụ
        /// </summary>
        public List<string> ServiceCode { get; set; }

        /// <summary>
        /// Loại sản phẩm
        /// </summary>
        public List<string> CategoryCode { get; set; }

        /// <summary>
        /// Sản phẩm
        /// </summary>
        public List<string> ProductCode { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }
       
        public string LoginCode { get; set; }
      
    }

    [Route("/api/v1/report/ReportCommissionTotal", "GET")]
    public class ReportCommissionTotalRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string Filter { get; set; }
        /// <summary>
        /// Đại lý tổng
        /// </summary>
        public string AgentCode { get; set; }

        public string LoginCode { get; set; }

    }


    [Route("/api/v1/report/ReportCommissionAgentDetail", "GET")]
    public class ReportCommissionAgentDetailRequest : PaggingBaseDto
    {       
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string TransCode { get; set; }

        /// <summary>
        /// Đại lý
        /// </summary>
        public string AgentCode { get; set; }

        /// <summary>
        /// Dịch vụ
        /// </summary>
        public string ServiceCode { get; set; }

        /// <summary>
        /// Loại sản phẩm
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// Sản phẩm
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int StatusPayment { get; set; }

        public string LoginCode { get; set; }

    }

    [Route("/api/v1/report/ReportCommissionAgentTotal", "GET")]
    public class ReportCommissionAgentTotalRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }     
        public string AgentCode { get; set; }
        public string LoginCode { get; set; }

    }

    [Route("/api/v1/report/ReportTopupRequestLogs", "GET")]
    public class ReportTopupRequestLogRequest : PaggingBaseDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string TransCode { get; set; }
        public string TransRef { get; set; }
        public string TransIndex { get; set; }
        public string ProviderCode { get; set; }
        public string PartnerCode { get; set; }
        public List<string> ServiceCode { get; set; }
        public List<string> CategoryCode { get; set; }
        public List<string> ProductCode { get; set; }
        public int Status { get; set; }
        public string File { get; set; }
    }

}
