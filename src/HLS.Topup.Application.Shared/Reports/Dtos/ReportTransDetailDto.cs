using System;
using System.Collections.Generic;
using System.Text;

namespace HLS.Topup.Report
{
    public class ReportTransDetailDto
    {
        public int Index { get; set; }
        public string StatusName { get; set; }
        public ReportStatus Status { get; set; }
        public string TransType { get; set; }        
        public string ServiceCode { get; set; }
        public string TransTypeName { get; set; }
        public string Vender { get; set; }
        public decimal Amount { get; set; }
        public decimal Quantity { get; set; }

        public decimal Fee { get; set; }

        public decimal Discount { get; set; }
        public decimal Price { get; set; }
        public decimal PriceIn { get; set; }
        public decimal PriceOut { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Balance { get; set; }
        public string AccountRef { get; set; }
        public string AccountCode { get; set; }
        public string TransCode { get; set; }

        public string PaidTransCode { get; set; }
        public string UserProcess { get; set; }        
        public string CategoryCode { get; set; }
        public DateTime CreatedDate { get; set; }

        public string RequestTransSouce { get; set; }
        public string TransTransSouce { get; set; }       
        public string TransNote { get; set; }
		
        public string Description { get; set; }

        public string ExtraInfo { get; set; }

        public int Print { get; set; }
    }


    public class ReportDebtDetailDto
    {
        public int Index { get; set; }
        #region 1.Thông tin loại giao dịch
        public string ServiceCode { get; set; }

        #endregion

        #region 2.Thông tin về số tiền,mã giao dich, thời gian, trạng thái

        public string RequestRef { get; set; }
        public string TransCode { get; set; }
        public decimal Price { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount
        {
            get; set;
        }
        public DateTime CreatedTime { get; set; }
        public string ServiceName { get; set; }

        public string Description { get; set; }

        #endregion

        #region 3.Phần thanh toán +/- tiền tài khoản
        public decimal Balance { get; set; }
        public decimal LimitBalance { get; set; }

        #endregion

        #region 5.Thông tin cơ bản tài khoản sale
        public string AccountCode { get; set; }

        public AccountItemDto AccountItem { get; set; }

        #endregion

    }


    public class ReportRefundDetailDto
    {
        #region 1.Đại lý

        public int Index { get; set; }
        public string AgentCode { get; set; }

        public string AgentInfo { get; set; }

        #endregion

        #region 2.Dịch vụ

        public string ServiceCode { get; set; }

        public string ServiceName { get; set; }

        #endregion

        #region 3.Loại sản phẩm

        public string CategoryCode { get; set; }

        public string CategoryName { get; set; }

        #endregion

        #region 4.Sản phẩm

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        #endregion

        #region 7.Thông tin về số tiền,mã giao dich, thời gian, trạng thái

        /// <summary>
        /// Thành tiền
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Thời gian
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// Mã giao dịch
        /// </summary>
        public string TransCode { get; set; }

        /// <summary>
        /// Mã giao dịch gốc
        /// </summary>
        public string TransCodeSouce { get; set; }

        /// <summary>
        /// Tên cửa hàng
        /// </summary>
        public string AgentName { get; set; }

        #endregion
    }

    public class ReportTransferDetailDto
    {
        public int Index { get; set; }

        #region 1.Loại đại lý
        public string AgentTypeName { get; set; }

        public int AgentType { get; set; }

        #endregion

        #region 2.Đại lý
        public string AgentReceiveCode { get; set; }

        public string AgentReceiveInfo { get; set; }

        public string AgentTransfer { get; set; }

        public string AgentTransferInfo { get; set; }

        #endregion

        #region 3.Dịch vụ

        public string ServiceCode { get; set; }

        public string ServiceName { get; set; }

        #endregion

        #region 4.Thông tin về số tiền,mã giao dich, thời gian

        /// <summary>
        /// Thời gian
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Thành tiền
        /// </summary>
        public decimal Price { get; set; }
        public string TransCode { get; set; }
        public string Messager { get; set; }


        #endregion
    }
    public class ReportServiceDetailDto
    {
        public int Index { get; set; }
        #region 1.Loại đại lý
        public string AgentTypeName { get; set; }

        public int AgentType { get; set; }

        #endregion

        #region 2.Đại lý
        public string AgentCode { get; set; }

        public string AgentInfo { get; set; }

        public string StaffCode { get; set; }

        public string StaffInfo { get; set; }

        #endregion

        #region 3.Nhà cung cấp

        public string VenderCode { get; set; }

        public string VenderName { get; set; }

        #endregion

        #region 4.Dịch vụ

        public string ServiceCode { get; set; }

        public string ServiceName { get; set; }

        #endregion

        #region 5.Loại sản phẩm

        public string CategoryCode { get; set; }

        public string CategoryName { get; set; }

        #endregion

        #region 6.Sản phẩm

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        #endregion

        #region 7.Thông tin về số tiền,mã giao dich, thời gian, trạng thái

        /// <summary>
        /// Đơn giá
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// Số lượng
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Số tiền chiết khấu
        /// </summary>
        public decimal Discount { get; set; }

        public decimal Fee { get; set; }

        /// <summary>
        /// Thành tiền
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Hoa hồng đại lý tổng
        /// </summary>
        public decimal CommistionAmount { get; set; }

        /// <summary>
        /// Thông tin đại lý tổng
        /// </summary>
        public string AgentParentInfo { get; set; }
        /// <summary>
        /// Số thụ hưởng
        /// </summary>
        public string ReceivedAccount { get; set; }
        /// <summary>
        /// Thời gian
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// Người thực hiện
        /// </summary>
        ///
        public string UserProcess { get; set; }

        /// <summary>
        /// Mã giao dịch NT sinh ra
        /// </summary>
        public string TransCode { get; set; }
        /// <summary>
        /// Mã giao dịch đối tác gọi sang
        /// </summary>
        public string RequestRef { get; set; }

        /// <summary>
        /// Mã giao dịch NT gọi sang đối tác
        /// </summary>
        public string PayTransRef { get; set; }

        public int Status { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public string StatusName { get; set; }

        public string Channel { get; set; }

        public string ReceiverType { get; set; }

        public string ProviderTransCode { get; set; }

        public string ProviderReceiverType { get; set; }

        public string ParentProvider { get; set; }

        #endregion
    }

    public class ReportServiceTotalDto
    {
        public int Index { get; set; }
        #region 1.Dịch vụ
        public string ServiceCode { get; set; }

        public string ServiceName { get; set; }

        #endregion

        #region 2.Loại sản phẩm

        public string CategoryCode { get; set; }

        public string CategoryName { get; set; }

        #endregion

        #region 3.Sản phẩm

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        #endregion

        #region 4.Thông tin về số tiền

        /// <summary>
        /// Đơn giá
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// Số lượng
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Số tiền chiết khấu
        /// </summary>
        public decimal Discount { get; set; }

        public decimal Fee { get; set; }

        /// <summary>
        /// Thành tiền
        /// </summary>
        public decimal Price { get; set; }

        public string Link { get; set; }

        #endregion
    }

    public class ReportServiceProviderDto
    {
        public int Index { get; set; }

        #region 0.Nhà cung cấp
        public string ProviderCode { get; set; }

        public string ProviderName { get; set; }

        #endregion

        #region 1.Dịch vụ
        public string ServiceCode { get; set; }

        public string ServiceName { get; set; }

        #endregion

        #region 2.Loại sản phẩm

        public string CategoryCode { get; set; }

        public string CategoryName { get; set; }

        #endregion

        #region 3.Sản phẩm

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        #endregion

        #region 4.Thông tin về số tiền

        /// <summary>
        /// Đơn giá
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// Số lượng
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Số tiền chiết khấu
        /// </summary>
        public decimal Discount { get; set; }

        public decimal Fee { get; set; }

        /// <summary>
        /// Thành tiền
        /// </summary>
        public decimal Price { get; set; }

        public string Link { get; set; }

        #endregion
    }


    public class ReportAgentBalanceDto
    {
        public int Index { get; set; }
        #region 1.Loại đại lý

        public int AgentType { get; set; }
        public string AgentTypeName { get; set; }

        #endregion

        #region 2.Đại lý
        public string AgentCode { get; set; }

        public string AgentInfo { get; set; }

        public string SaleCode { get; set; }

        public string SaleInfo { get; set; }

        public string SaleLeaderCode { get; set; }

        public string SaleLeaderInfo { get; set; }

        #endregion

        #region 3.Thông tin về số tiền

        /// <summary>
        /// Đầu kỳ
        /// </summary>
        public decimal BeforeAmount { get; set; }
        /// <summary>
        /// Tiền nạp
        /// </summary>
        public decimal InputAmount { get; set; }
        /// <summary>
        /// Phát sinh tăng
        /// </summary>
        public decimal AmountUp { get; set; }
        /// <summary>
        /// Bán hàng
        /// </summary>
        public decimal SaleAmount { get; set; }

        /// <summary>
        ///Phát sinh giảm khác
        /// </summary>
        public decimal AmountDown { get; set; }
        /// <summary>
        /// Số dư cuối kỳ
        /// </summary>
        public decimal AfterAmount { get; set; }

        public string Link { get; set; }

        #endregion
    }

    public class ReportRevenueAgentDto
    {
        public int Index { get; set; }

        #region 1.Loại đại lý
        public string AgentTypeName { get; set; }

        public int AgentType { get; set; }

        #endregion

        #region 2.Đại lý
        public string AgentCode { get; set; }

        public string AgentInfo { get; set; }

        #endregion

        #region 3.Thông tin về số tiền

        public string AgentName { get; set; }

        public string SaleInfo { get; set; }

        public string SaleCode { get; set; }

        public string SaleLeaderInfo { get; set; }

        public string SaleLeaderCode { get; set; }

        public string CityInfo { get; set; }

        public string DistrictInfo { get; set; }

        public string WardInfo { get; set; }

        public int CityId { get; set; }

        public int DistrictId { get; set; }

        public int WardId { get; set; }


        /// <summary>
        /// Số lượng
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Số tiền chiết khấu
        /// </summary>
        public decimal Discount { get; set; }

        public decimal Fee { get; set; }

        /// <summary>
        /// Thành tiền
        /// </summary>
        public decimal Price { get; set; }

        public string Link { get; set; }

        #endregion
    }

    public class ReportRevenueCityDto
    {
        public int Index { get; set; }

        #region 1.Thông tin về số tiền

        public string CityInfo { get; set; }

        public string DistrictInfo { get; set; }

        public string WardInfo { get; set; }

        public int CityId { get; set; }

        public int DistrictId { get; set; }

        public int WardId { get; set; }

        public decimal QuantityAgent { get; set; }
        /// <summary>
        /// Số lượng
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Số tiền chiết khấu
        /// </summary>
        public decimal Discount { get; set; }

        public decimal Fee { get; set; }

        /// <summary>
        /// Thành tiền
        /// </summary>
        public decimal Price { get; set; }

        public string LinkDetail { get; set; }

        public string LinkAgent { get; set; }

        #endregion
    }


    public class ReportTotalSaleAgentDto
    {
        public int Index { get; set; }
        #region 1.Loại đại lý
        public string AgentTypeName { get; set; }

        public int AgentType { get; set; }

        #endregion

        #region 2.Đại lý
        public string AgentCode { get; set; }

        public string AgentInfo { get; set; }

        #endregion

        #region 3.Thông tin về số tiền

        public string AgentName { get; set; }

        public string SaleInfo { get; set; }

        public string SaleLeaderInfo { get; set; }

        public string CityInfo { get; set; }

        public string DistrictInfo { get; set; }

        public string WardInfo { get; set; }

        public int CityId { get; set; }
        public int WardId { get; set; }
        public int DistrictId { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Số tiền chiết khấu
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Phí
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// Thành tiền
        /// </summary>
        public decimal Price { get; set; }

        public string Link { get; set; }

        #endregion
    }

    public class ReportRevenueActiveDto
    {
        public int Index { get; set; }
        #region 1.Loại đại lý
        public string AgentTypeName { get; set; }

        public int AgentType { get; set; }

        #endregion

        #region 2.Đại lý
        public string AgentCode { get; set; }

        public string AgentInfo { get; set; }

        #endregion

        #region 3.Thông tin về số tiền

        public string AgentName { get; set; }

        public string SaleInfo { get; set; }

        public string SaleLeaderInfo { get; set; }

        public string CityInfo { get; set; }


        public string DistrictInfo { get; set; }

        public string WardInfo { get; set; }

        public int CityId { get; set; }

        /// <summary>
        /// Quận huyện
        /// </summary>
        public int DistrictId { get; set; }

        /// <summary>
        /// Phường/xã
        /// </summary>
        public int WardId { get; set; }

        public string IdIdentity { get; set; }

        /// <summary>
        ///Số tiền nạp trong kỳ
        /// </summary>
        public decimal Deposit { get; set; }
        /// <summary>
        /// Số tiền bán trong kỳ
        /// </summary>
        public decimal Sale { get; set; }

        public string Status { get; set; }

        public string LinkDeposit { get; set; }
        public string LinkDetail { get; set; }

        #endregion
    }

    public class ReportItemTotalDay
    {
        public DateTime CreatedDay { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal IncDeposit { get; set; }
        public decimal IncOther { get; set; }
        public decimal DecPayment { get; set; }
        public decimal DecOther { get; set; }
        public decimal BalanceAfter { get; set; }
    }

    public class ReportItemTotalDebt
    {
        public int Index { get; set; }
        public string SaleCode { get; set; }
        public string SaleInfo { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal IncDeposit { get; set; }
        public decimal DecPayment { get; set; }
        public decimal BalanceAfter { get; set; }
        public string LinkDebt { get; set; }
        public string LinkPayDebt { get; set; }
    }  

    /// <summary>
    /// Thông tin object của dịch vụ
    /// </summary>
    public class ServiceItemDto
    {
        #region 2.Thông tin loại giao dịch
        public int? ServiceId { get; set; }
        public string ServiceCode { get; set; }
        public string ServicesName { get; set; }

        #endregion
    }

    /// <summary>
    /// Thông Tin Object của sản phẩm
    /// </summary>
    public class ProductItemDto
    {
        #region 3.Thông tin về sản phẩm
        public int? ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }

        #endregion
    }

    /// <summary>
    /// Thông tin object của tài khoản
    /// </summary>
    public class AccountItemDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string ParentCode { get; set; }
        public string TreePath { get; set; }
        public int AccountType { get; set; }
        public int AgentType { get; set; }
        public string AgentName { get; set; }
        public string Gender { get; set; }
        public int NetworkLevel { get; set; }      
    }

    /// <summary>
    /// Thông tin nhà cung cấp
    /// </summary>
    public class ProviderItemDto
    {
        #region 4.Thông tin về nhà cung cấp

        public int? ProvidersId { get; set; }
        public string ProvidersCode { get; set; }
        public string ProvidersName { get; set; }

        #endregion
    }

    /// <summary>
    /// Thông tin nhà cung cấp
    /// </summary>
    public class VenderItemDto
    {
        #region 4.Thông tin về nhà cung cấp

        public int? VenderId { get; set; }
        public string VenderCode { get; set; }
        public string VenderName { get; set; }

        #endregion
    }
  
    public class GetRevenueInDayDto
    {
        public decimal Quantity { get; set; }
        public decimal Revenue { get; set; }
        public decimal SalePrice { get; set; }
    }

    public class ReportRevenueDashboardDay
    {
        public DateTime CreatedDay { get; set; }
        public decimal Revenue { get; set; }
        public decimal Discount { get; set; }
    }

    public class ReportRevenueCommistionDashDay
    {
        public DateTime CreatedDay { get; set; }
        public string DayText { get; set; }
        public decimal Revenue { get; set; }
        public decimal Commission { get; set; }
    }

    public class DashRevenueItem
    {
        public decimal y { get; set; }
        public decimal d { get; set; }

        public string indexLabel { get; set; }

        public string label { get; set; }

        public string toolTipContent { get; set; }
    }

    public class DashAgentGenerals
    {

        public List<DashRevenueItem> Revenues { get; set; }

        public List<DashRevenueItem> Commistions { get; set; }
        public int Length { get; set; }
    }

    public class ReportComparePartnerDto
    {

        #region 1.Dịch vụ
        public string ServiceCode { get; set; }

        public string ServiceName { get; set; }

        #endregion

        #region 2.Loại sản phẩm

        public string CategoryCode { get; set; }

        public string CategoryName { get; set; }

        #endregion

        #region 3.Sản phẩm

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        #endregion

        #region 4.Thông tin về số tiền

        /// <summary>
        /// Đơn giá
        /// </summary>
        public decimal Value { get; set; }
        public decimal ProductValue { get; set; }
        /// <summary>
        /// Số lượng
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Số tiền chiết khấu
        /// </summary>
        public decimal Discount { get; set; }
        public decimal DiscountRate { get; set; }

        public decimal Fee { get; set; }

        public string FeeText { get; set; }

        /// <summary>
        /// Thành tiền
        /// </summary>
        public decimal Price { get; set; }

        public decimal DiscountVat { get; set; }

        public decimal DiscountNoVat { get; set; }

        public string ReceiverType { get; set; }

        public string Note { get; set; }


        #endregion
    }


    public class ReportBalancePartnerDto
    {
        public int Index { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }

        public decimal Price { get; set; }
    }

    public class ReportComparePartnerExportInfo
    {

        public string Title { get; set; }
        public string PeriodCompare { get; set; }
        public string Provider { get; set; }
        public string Contract { get; set; }
        public string FullName { get; set; }
        public string PeriodPayment { get; set; }
        public List<ReportComparePartnerDto> PinCodeItems { get; set; }
        public ReportComparePartnerDto SumPinCodes { get; set; }
        public int TotalRowsPinCode { get; set; }

        public List<ReportComparePartnerDto> PinGameItems { get; set; }
        public ReportComparePartnerDto SumPinGames { get; set; }
        public int TotalRowsPinGame { get; set; }

        public List<ReportComparePartnerDto> TopupItems { get; set; }
        public ReportComparePartnerDto SumTopup { get; set; }
        public int TotalRowsTopup { get; set; }

        public List<ReportComparePartnerDto> TopupPrepaIdItems { get; set; }
        public ReportComparePartnerDto SumTopupPrepaId { get; set; }
        public int TotalRowsTopupPrepaId { get; set; }

        public List<ReportComparePartnerDto> TopupPostpaIdItems { get; set; }
        public ReportComparePartnerDto SumTopupPostpaId { get; set; }
        public int TotalRowsTopupPostpaId { get; set; }

        public List<ReportComparePartnerDto> DataItems { get; set; }
        public ReportComparePartnerDto SumData { get; set; }
        public int TotalRowsData { get; set; }

        public List<ReportComparePartnerDto> PayBillItems { get; set; }

        public ReportComparePartnerDto SumPayBill { get; set; }

        public List<ReportBalancePartnerDto> BalanceItems { get; set; }

        public int TotalRowsBalance { get; set; }

        public int TotalRowsPayBill { get; set; }

        public string TotalFeePartner { get; set; }

        public string TotalFeePartnerChu { get; set; }

        public bool IsAccountApi { get; set; }

        public static string GetIndex(int index)
        {
            string txt = "";
            switch (index)
            {
                case 1:
                    txt = "I";
                    break;
                case 2:
                    txt = "II";
                    break;
                case 3:
                    txt = "III";
                    break;
                case 4:
                    txt = "IV";
                    break;
                case 5:
                    txt = "V";
                    break;
                case 6:
                    txt = "VI";
                    break;
                case 7:
                    txt = "VII";
                    break;
                case 8:
                    txt = "VIII";
                    break;
                case 9:
                    txt = "IX";
                    break;
                case 10:
                    txt = "X";
                    break;
            }

            return txt;
        }

        public class ValidateSearch
        {
            public string Code { get; set; }
            public string Message { get; set; }
        }

        public class ValidateSearchInput
        {
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
            public string Type { get; set; }
            public string ReportType { get; set; }
        }

        public class ReportTopupRequestLogDto
        {
            public string TransCode { get; set; }
            public string TransRef { get; set; }
            public decimal TransAmount { get; set; }
            public string ReceiverInfo { get; set; }
            public TransRequestStatus Status { get; set; }
            public DateTime RequestDate { get; set; }
            public DateTime ModifiedDate { get; set; }
            public DateTime AddedAtUtc { get; set; }
            public string Vendor { get; set; }
            public string CategoryCode { get; set; }
            public string ProductCode { get; set; }
            public string ProviderCode { get; set; }
            public string ResponseInfo { get; set; }
            public string ServiceCode { get; set; }
            public string PartnerCode { get; set; }
            public string ReferenceCode { get; set; }
            public string TransIndex { get; set; }
            public int? ProviderSetTransactionTimeout { get; set; }
            public int? ProviderMaxWaitingTimeout { get; set; }
            public bool? IsEnableResponseWhenJustReceived { get; set; }
            public string StatusResponseWhenJustReceived { get; set; }
            public int? WaitingTimeResponseWhenJustReceived { get; set; }

            //ExtraInfo
            public string Result { get; set; }
            public string ResponseCode { get; set; }
            public string ResponseMessage { get; set; }
            public string ReceiverType { get; set; }
            public string StatusName { get; set; }
        }

        public enum TransRequestStatus : byte
        {
            Init = 0,
            Success = 1,
            Fail = 3,
            Timeout = 4,
            Cancel = 2
        }
    }
}
