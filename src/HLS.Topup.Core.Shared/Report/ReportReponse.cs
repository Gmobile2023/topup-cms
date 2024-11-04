﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HLS.Topup.Report
{
    public class ReportBalanceTotalDto
    {
        public string AccountCode { get; set; }
        public string AccountInfo { get; set; }
        public int AgentType { get; set; }        
        public decimal Credited { get; set; }
        public decimal Debit { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }        
    }

    public class ReportCardStockInventoryDto
    {
        public int Index { get; set; }
        public string StockCode { get; set; }
        public int CardValue { get; set; }
        public string StockType { get; set; }
        public string ProductCode { get; set; }
        public string Telco { get; set; }
        public int Increase { get; set; }
        public int Decrease { get; set; }
        public int InventoryBefore { get; set; }
        public int InventoryAfter { get; set; }
        public byte Status { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public int CampareInventory { get; set; }
        public int CampareTrans { get; set; }
        public int ExportForTrans { get; set; }
    }
    public class ReportCardStockHistoriesDto
    {
        public int Index { get; set; }
        public string StockCode { get; set; }
        public string StockType { get; set; }
        public string ProductCode { get; set; }
        public int CardValue { get; set; }
        public string Serial { get; set; }
        public string Telco { get; set; }
        public int Increase { get; set; }
        public int Decrease { get; set; }
        public int InventoryBefore { get; set; }
        public int InventoryAfter { get; set; }
        public byte Status { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public string PartnerCode { get; set; }
        public string TransType { get; set; }
        public string TransRef { get; set; }
        public string TransCode { get; set; }
        public string InventoryType { get; set; }
    }

    public class ReportCardStockImExPortDto
    {
        public int Index { get; set; }
        public string StoreCode { get; set; }
        public string ServiceName { get; set; }
        public string CategoryName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProviderCode { get; set; }
        public string ProviderName { get; set; }
        public int CardValue { get; set; }
        public int IncreaseSupplier { get; set; }
        public int IncreaseOther { get; set; }
        public int Sale { get; set; }
        public int ExportOther { get; set; }
        public int Before { get; set; }
        public int After { get; set; }
        public int Current { get; set; }
    }

    public class ReportCardStockAutoDto
    {
        public string ServiceName { get; set; }
        public string CategoryName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int CardValue { get; set; }
        public int Before_Sale { get; set; }
        public int Import_Sale { get; set; }
        public int Export_Sale { get; set; }
        public int After_Sale { get; set; }
        public decimal Monney_Sale { get; set; }
        public int Before_Temp { get; set; }
        public int Import_Temp { get; set; }
        public int Export_Temp { get; set; }
        public int After_Temp { get; set; }
        public decimal Monney_Temp { get; set; }
    }

    public class ReportItemDetailDto
    {
        #region 1.Thông tin cơ bản tài khoản thực hiện giao dịch

        public string PerformAccount { get; set; }
        public string PerformInfo { get; set; }
        public int PerformAgentType { get; set; }

        #endregion

        #region 2.Thông tin loại giao dịch

        public string TransType { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }

        #endregion

        #region 3.Thông tin về sản phẩm

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }

        #endregion

        #region 4.Thông tin về nhà cung cấp

        public string ProvidersCode { get; set; }
        public string ProvidersInfo { get; set; }
        public string VenderCode { get; set; }
        public string VenderName { get; set; }

        #endregion

        #region 5.Thông tin về số tiền,mã giao dich, thời gian, trạng thái

        public string RequestRef { get; set; }
        public string TransCode { get; set; }
        public decimal Quantity { get; set; }
        public double Amount { get; set; }
        public double Discount { get; set; }
        public double Price { get; set; }
        public double TotalPrice { get; set; }
        public double Fee { get; set; }
        public double PriceIn { get; set; }
        public double PriceOut { get; set; }
        public DateTime CreatedTime { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        public double? CommissionAmount { get; set; }
        public int? CommissionStatus { get; set; }
        public DateTime? CommissionDate { get; set; }

        public string CommissionPaidCode { get; set; }

        #region 5.1.Phần thanh toán +/- tiền tài khoản

        public string PaidTransCode { get; set; }
        public double? PaidAmount { get; set; }
        public string RequestTransSouce { get; set; }
        public string TransTransSouce { get; set; }
        public double? PerformBalance { get; set; }
        public double? Balance { get; set; }
        public string FeeText { get; set; }

        #endregion

        #region 5.2.Phần Topup/Lấy mã thẻ

        public string PayTransRef { get; set; }
        public string ReceivedAccount { get; set; }

        #endregion

        public ReportStatus Status { get; set; }
        public string TransNote { get; set; }
        public string ExtraInfo { get; set; }
        public string Channel { get; set; }
        public string TextDay { get; set; }

        #endregion

        #region 6.Thông tin cơ bản tài khoản nhận tiền(Dùng cho giao dịch chuyển tiền ngang)/Tài khoản đại lý

        public string AccountCode { get; set; }
        public string AccountInfo { get; set; }
        public int AccountAccountType { get; set; }
        public int AccountAgentType { get; set; }
        public string AccountAgentName { get; set; }
        public int AccountCityId { get; set; }
        public string AccountCityName { get; set; }
        public int AccountDistrictId { get; set; }
        public string AccountDistrictName { get; set; }
        public int AccountWardId { get; set; }
        public string AccountWardName { get; set; }
        public string SaleCode { get; set; }
        public string SaleInfo { get; set; }
        public string SaleLeaderCode { get; set; }
        public string SaleLeaderInfo { get; set; }
        public string ReceiverType { get; set; }

        #endregion
    }

    public enum ReportStatus : byte
    {
        Process = 0, //Đang xử lý
        Error = 3, //Lỗi
        Success = 1, //Thành công
        TimeOut = 2 //Chưa xác định
    }

}
