using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace HLS.Topup.Reports.Dtos
{
    public class GetReportTransDetailInput : PagedAndSortedResultRequestDto
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
        public string AccountCode
        {
            get; set;
        }

        public int Type { get; set; }
    }

    public class TransRequestByTransCodeInput
    {
        public string TransCode { get; set; }      

        public string TransType { get; set; }
    }


    public class GetReportdebtDetailInput : PagedAndSortedResultRequestDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string TransCode { get; set; }
        public string ServiceCode { get; set; }
        public string AccountCode { get; set; }
    }


    public class GetReportRefundDetailInput : PagedAndSortedResultRequestDto
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

    }
    public class GetReportServiceDetailInput : PagedAndSortedResultRequestDto
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
        /// Nhân viên quản lý
        /// </summary>
        public string UserSaleCode { get; set; }

        /// <summary>
        /// Sale Leader
        /// </summary>
        public string UserSaleLeaderCode { get; set; }

        /// <summary>
        /// Mã giao dịch NT
        /// </summary>
        public string TransCode { get; set; }


        /// <summary>
        /// Mã giao dịch đối tác gọi sang
        /// </summary>
        public string RequestRef { get; set; }


        /// <summary>
        /// Mã giao dịch NT gọi sang nhà cung cấp
        /// </summary>
        public string PayTransRef { get; set; }

        /// <summary>
        /// Nhà cung cấp
        /// </summary>
        public List<string> VenderCode { get; set; }

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
        /// Loại thuê bao
        /// </summary>
        public string ReceiverType { get; set; }
        public string ProviderReceiverType { get; set; }
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
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }

        public int TenantId { get; set; }

        public string ProviderTransCode { get; set; }        
    }

    public class GetReportTransferDetailInput : PagedAndSortedResultRequestDto
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
        public string AgentTransferCode { get; set; }

        public string AgentReceiveCode { get; set; }
    }

    public class GetReportServiceTotalInput : PagedAndSortedResultRequestDto
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

    public class GetReportServiceProviderInput : PagedAndSortedResultRequestDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Nhà cung cấp
        /// </summary>
        public List<string> ProviderCode { get; set; }

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
    }


    public class GetReportAgentBalanceInput : PagedAndSortedResultRequestDto
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
    }

    public class GetReportRevenueAgentInput : PagedAndSortedResultRequestDto
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

    }

    public class GetReportRevenueCityInput : PagedAndSortedResultRequestDto
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
    }

    public class GetReportTotalSaleAgentInput : PagedAndSortedResultRequestDto
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

    }

    public class GetReportRevenueActiveInput : PagedAndSortedResultRequestDto
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

    }

    public class GetReportRevenueDashboardInput : PagedAndSortedResultRequestDto
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

    }

    public class GetDashboardRevenueInput
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

    }


    public class GetDashAgentGeneralInput
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

        /// <summary>
        /// Đại lý con
        /// </summary>
        public string AgentCode { get; set; }

    }

    public class GetDashAgentGeneralboardInput : PagedAndSortedResultRequestDto
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

        /// <summary>
        /// Đại lý con
        /// </summary>
        public string AgentCode { get; set; }

    }

    public class GetReportTotalDayInput : PagedAndSortedResultRequestDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string AccountCode
        {
            get; set;
        }
    }

    public class GetRevenueInDayInput
    {

    }
    public class GetReportTotalDebtInput : PagedAndSortedResultRequestDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string AccountCode
        {
            get; set;
        }
    }

    public class GetReportComparePartnerInput : PagedAndSortedResultRequestDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Đại lý
        /// </summary>
        public string AgentCode { get; set; }


        public string Email { get; set; }

        /// <summary>
        /// Loại
        /// </summary>
        public string Type { get; set; }

        public string ChangerType { get; set; }

        public string ServiceCode { get; set; }

    }

    public class GetReportBalancePartnerInput : PagedAndSortedResultRequestDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Đại lý
        /// </summary>
        public string AgentCode { get; set; }

        /// <summary>
        /// Loại
        /// </summary>
        public string Type { get; set; }

    }


    public class SendMailComparePartnerInput
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        /// <summary>
        /// Đại lý
        /// </summary>
        public string AgentCode { get; set; }


        public string Email { get; set; }

    }


    public class GetReportAccountBalanceInput
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Đại lý
        /// </summary>
        public string AgentCode { get; set; }

    }

    public class GetReportCommissionDetailInput : PagedAndSortedResultRequestDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Tìm kiếm chung
        /// </summary>
        public string Filter { get; set; }

        public string TransCode { get; set; }
        /// <summary>
        /// Đại lý
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
    }




    public class GetReportCommissionTotalInput : PagedAndSortedResultRequestDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Đại lý tổng
        /// </summary>
        public string AgentCode { get; set; }

        /// <summary>
        /// Tìm kiếm chung
        /// </summary>
        public string Filter { get; set; }

    }


    public class GetReportCommissionAgentDetailInput : PagedAndSortedResultRequestDto
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
    }


    public class GetReportCommissionAgentTotalInput : PagedAndSortedResultRequestDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Đại lý tổng
        /// </summary>
        public string AgentCode { get; set; }

    }

    public class GetReportTopupRequestDetailInput : PagedAndSortedResultRequestDto
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
        /// Nhân viên quản lý
        /// </summary>
        public string UserSaleCode { get; set; }

        /// <summary>
        /// Sale Leader
        /// </summary>
        public string UserSaleLeaderCode { get; set; }

        /// <summary>
        /// Mã giao dịch NT
        /// </summary>
        public string TransCode { get; set; }


        /// <summary>
        /// Mã giao dịch đối tác gọi sang
        /// </summary>
        public string RequestRef { get; set; }


        /// <summary>
        /// Mã giao dịch NT gọi sang nhà cung cấp
        /// </summary>
        public string PayTransRef { get; set; }

        /// <summary>
        /// Nhà cung cấp
        /// </summary>
        public string VenderCode { get; set; }

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

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }

        public int TenantId { get; set; }

        public string ReceiverType { get; set; }
    }
}
