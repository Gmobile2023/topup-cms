using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Stock;
using ServiceStack;

namespace HLS.Topup.RequestDtos
{
    [Route("/api/v1/stock/card_batch")]
    public class CardBatchCreateRequest : IUserInfoRequest
    {
        public string BatchCode { get; set; }

        // public string BatchName { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte Status { get; set; }
        public string Description { get; set; }
        [DataMember(Name = "provider")] public string ProviderCode { get; set; }

        [DataMember(Name = "vendor")] public string VendorCode { get; set; }

        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/stock/card_batch")]
    public class CardBatchUpdateRequest : IUserInfoRequest
    {
        public Guid Id { get; set; }
        public string BatchCode { get; set; }
        public string BatchName { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte Status { get; set; }
        public string Description { get; set; }
        [DataMember(Name = "provider")] public string ProviderCode { get; set; }

        [DataMember(Name = "vendor")] public string VendorCode { get; set; }

        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/stock/card_batch")]
    public class CardBatchDeleteRequest
    {
        public Guid Id { get; set; }
    }

    [Route("/api/v1/stock/card_batches")]
    public class CardBatchGetListRequest : PaggingBaseDto, IUserInfoRequest
    {
        public string Filter { get; set; }

        public string BatchCode { get; set; }

        //public string BatchName { get; set; }
        //public string ProductCode { get; set; }
        public byte Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        [DataMember(Name = "provider")] public string Provider { get; set; }

        [DataMember(Name = "vendor")] public string Vendor { get; set; }
        public string ImportType { get; set; }

        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }


    [Route("/api/v1/stock/card_batch")]
    public class CardBatchGetRequest : IUserInfoRequest
    {
        public Guid Id { get; set; }
        public string BatchCode { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/stock/card")]
    public class CardGetRequest : IUserInfoRequest
    {
        public Guid Id { get; set; }
        public string Serial { get; set; }
        public string ProductCode { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/stock/cardfull")]
    public class CardGetFullRequest : IUserInfoRequest
    {
        public Guid Id { get; set; }
        public string Serial { get; set; }
        public string ProductCode { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/stock/cards")]
    public class CardGetListRequest : PaggingBaseDto, IUserInfoRequest
    {
        public string Filter { get; set; }

        public string BatchCode { get; set; }
        public string StockCode { get; set; }
        public string Serial { get; set; }
        public string CardCode { get; set; }

        public string ProviderCode { get; set; }
        public string CategoryCode { get; set; }
        public string ServiceCode { get; set; }

        public DateTime? FromExpiredDate { get; set; }
        public DateTime? ToExpiredDate { get; set; }

        public DateTime? FromImportDate { get; set; }
        public DateTime? ToImportDate { get; set; }

        public DateTime? FromExportedDate { get; set; }
        public DateTime? ToExportedDate { get; set; }

        public int? FormCardValue { get; set; }
        public int? ToCardValue { get; set; }


        public byte Status { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }


    [Route("/api/v1/stock/cards")]
    public class CardImportListRequest : IUserInfoRequest
    {
        public List<CardItem> CardItems { get; set; }
        public string Description { get; set; }

        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/stock/cards-file-import")]
    public class CardImportFileModel : IUserInfoRequest
    {
        public List<CardImportFileItemModel> Data { get; set; }
        public string Description { get; set; }
        public string Provider { get; set; }

        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    public class CardImportFileItemModel
    {
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public decimal CardValue { get; set; }
        public int Quantity { get; set; }
        public float Discount { get; set; }
        public List<CardImportFileLineModel> Cards { get; set; }
    }

    public class CardImportFileLineModel
    {
        public string Serial { get; set; }
        public string CardCode { get; set; }
        public DateTime ExpiredDate { get; set; }
    }

    [Route("/api/v1/backend/card-import-provider")]    
    public class CardImportApiRequest : IUserInfoRequest
    {
        public string ProviderCode { get; set; }
        public string Description { get; set; }
        public List<CardImportApiItemRequest> CardItems { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
        public DateTime? ExpiredDate { get; set; }
    }

    [Route("/api/v1/stock/cards-api-check-trans")]
    public class StockCardApiCheckTransRequest 
    {
        public string Provider { get; set; }
        public string TransCodeProvider { get; set; }      
    }

    public class CardImportApiItemRequest
    {
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public decimal CardValue { get; set; }
        public int Quantity { get; set; }
        public float Discount { get; set; }
    }

    public class CardItem
    {
        public Guid Id { get; set; }
        public string CardCode { get; set; }
        public string Serial { get; set; }
        public DateTime ExpiredDate { get; set; }
        public DateTime ImportedDate { get; set; }
        public DateTime ExportedDate { get; set; }
        public byte Status { get; set; }
        public DateTime? UsedDate { get; set; }
        public int CardValue { get; set; }
        public string BatchCode { get; set; }
        public string StockCode { get; set; }
    }

    [Route("/api/v1/stock/card")]
    public class CardImportRequest : IUserInfoRequest
    {
        public string BatchCode { get; set; }
        public string ProductCode { get; set; }
        public CardItem CardItem { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/stock/card-stock")]
    public class CardStockCreateRequest : IUserInfoRequest
    {
        public virtual string StockCode { get; set; }
        public virtual string ServiceCode { get; set; }
        public virtual string CategoryCode { get; set; }
        public virtual decimal CardValue { get; set; }
        public virtual int InventoryLimit { get; set; }
        public virtual int MinimumInventoryLimit { get; set; }
        public virtual string Description { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/stock/card-stock")]
    public class CardStockUpdateRequest : IUserInfoRequest
    {
        public virtual string StockCode { get; set; }
        public virtual string ServiceCode { get; set; }
        public virtual string CategoryCode { get; set; }
        public virtual string ProductCode { get; set; }

        public virtual decimal CardValue { get; set; }

        public virtual int InventoryLimit { get; set; }

        public virtual int MinimumInventoryLimit { get; set; }
        public virtual string Description { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/stock/card-stock-quantity")]
    public class CardStockUpdateQuantityRequest 
    {
        public string StockCode { get; set; }
        public string KeyCode { get; set; }
        public int Inventory { get; set; }
    }

    [Route("/api/v1/stock/cards_exchange")]
    public class CardStockTransferRequest : IUserInfoRequest
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string SrcStockCode { get; set; }
        public string DesStockCode { get; set; }
        [DataMember(Name = "vendor")] public string VendorCode { get; set; }
        public string BatchCode { get; set; }
        public int? CardValue { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public byte Status { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/stock/cards_stock_transfer")]
    public class CardsStockTransferRequest : IUserInfoRequest
    {
        public virtual Guid Id { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    /// <summary>
    /// danh sách kho
    /// </summary>
    [Route("/api/v1/stock/card-stock-list")]
    public class CardStockGetListRequest : PaggingBaseDto, IUserInfoRequest
    {
        public string Filter { get; set; }
        public string StockCode { get; set; }
        public int MinCardValue { get; set; }
        public int MaxCardValue { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public int Status { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    /// <summary>
    /// chi tiết kho
    /// </summary>
    [Route("/api/v1/stock/card-stock")]
    public class CardStockGetRequest : IUserInfoRequest
    {
        public virtual string StockCode { get; set; }
        public virtual string ProductCode { get; set; }
        public virtual decimal CardValue { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/backend/sim")]
    public class SimCreateRequest : IUserInfoRequest
    {
        public virtual string SimNumber { get; set; }
        public virtual string ProductCode { get; set; }

        public virtual string Iccid { get; set; }

        public virtual decimal SimBalance { get; set; }

        public virtual int ComPort { get; set; }
        public virtual int BaudRate { get; set; }
        public virtual byte Status { get; set; }
        public virtual string Description { get; set; }

        public virtual bool IsSimPostpaid { get; set; }
        public virtual bool IsInprogress { get; set; }
        public int TransTimesInDay { get; set; }
        public CommonConst.SimAppType? SimAppType { get; set; }

        public string MyViettelPass { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/backend/sims", "POST")]
    public class SimCreateManyRequest : IUserInfoRequest
    {
        public List<SimCreateRequest> Sims { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/backend/sim")]
    public class SimUpdateRequest : IUserInfoRequest
    {
        public virtual string SimNumber { get; set; }
        public virtual string ProductCode { get; set; }
        public virtual string Iccid { get; set; }
        public virtual decimal SimBalance { get; set; }
        public virtual int ComPort { get; set; }
        public virtual int BaudRate { get; set; }
        public virtual byte Status { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsSimPostpaid { get; set; }
        public virtual bool IsInprogress { get; set; }
        public CommonConst.SimAppType? SimAppType { get; set; }
        public int TransTimesInDay { get; set; }
        public string MyViettelPass { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/backend/sims")]
    public class SimGetListRequest : PaggingBaseDto, IUserInfoRequest
    {
        public virtual string SimNumber { get; set; }
        public virtual string ProductCode { get; set; }
        public virtual string Iccid { get; set; }
        public virtual decimal? FromBalance { get; set; }
        public virtual decimal? ToBalance { get; set; }
        public virtual int? ComPort { get; set; }
        public virtual int? BaudRate { get; set; }
        public virtual byte Status { get; set; }
        public virtual string WorkerAppName { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/backend/sim")]
    public class SimGetRequest : IUserInfoRequest
    {
        public virtual string SimNumber { get; set; }
        public virtual string Iccid { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }


    [Route("/api/v1/backend/topups")]
    public class TopupsListRequest : PaggingBaseDto, IUserInfoRequest
    {
        public virtual string MobileNumber { get; set; }
        public virtual string TransRef { get; set; }
        public virtual string TransCode { get; set; }
        public virtual string ProviderTransCode { get; set; }
        public virtual List<byte?> Status { get; set; }
        public virtual DateTime? FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public List<string> ServiceCodes { get; set; }
        public List<string> CategoryCodes { get; set; }
        public List<string> ProductCodes { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public List<string> ProviderCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public string Filter { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
        public CommonConst.SaleType SaleType { get; set; }
        public string ReceiverType { get; set; }
        public string ProviderResponseCode { get; set; }
        public string ReceiverTypeResponse { get; set; }
        public string ParentProvider { get; set; }
        
    }

    [Route("/api/v1/backend/topup", "GET")]
    public class GetSaleRequest
    {
        public string Filter { get; set; }
    }

    [Route("/api/v1/backend/topup/status")]
    public class TopupsUpdateStatusRequest : IUserInfoRequest
    {
        public virtual string TransCode { get; set; }
        public virtual byte Status { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }


    [Route("/api/v1/stock/card/status")]
    public class CardUpdateStatusRequest : IUserInfoRequest
    {
        public virtual Guid Id { get; set; }
        public virtual byte CardStatus { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/stock/card")]
    public class CardUpdateRequest : IUserInfoRequest
    {
        public Guid Id { get; set; }
        public virtual string Serial { get; set; }
        public virtual string CardCode { get; set; }
        public int CardValue { get; set; }
        public byte Status { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/gateway/topup")]
    public class TopupRequest : IUserInfoRequest
    {
        public string ReceiverInfo { get; set; }
        public int Amount { get; set; }
        public string TransCode { get; set; }
        public string ProductCode { get; set; } //ProductCode nhà mạng
        public string ServiceCode { get; set; } //Dịch  vụ
        public string CategoryCode { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.Channel Channel { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/gateway/paybatch", "POST")]
    public class PayBatchRequest : IUserInfoRequest
    {
        public List<PayBatchItemDto> Items { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.Channel Channel { get; set; }
        public string TransRef { get; set; }
        public string BatchType { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/gateway/batchLot_Stop", "POST")]
    public class BatchLotStopRequest
    {
        public string BatchCode { get; set; }
        public string AccountCode { get; set; }
        public bool IsStaff { get; set; }
    }

    [Route("/api/v1/gateway/topup", "DELETE")]
    public class TopupCancelRequest : IUserInfoRequest
    {
        public string TransCode { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/backend/refund", "POST")]
    public class TransactionRefundRequest : IUserInfoRequest
    {
        public string TransCode { get; set; }
        public string TransRef { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/gateway/topup/topup_priority", "PUT")]
    public class TopupPriorityRequest : IUserInfoRequest
    {
        public string TransCode { get; set; }
        public decimal DiscountPriority { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    [Route("/api/v1/gateway/card/card_sale", "POST")]
    public class CardSaleRequest : IUserInfoRequest
    {
        public string TransCode { get; set; }
        public string CategoryCode { get; set; }
        public int Quantity { get; set; }
        public int CardValue { get; set; }
        public string Email { get; set; }
        public string ProductCode { get; set; }
        public string ServiceCode { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.Channel Channel { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }


    [Route("/api/v1/stock/get-card-transfer-info", "GET")]
    public class GetCardInfoTransferRequest : IUserInfoRequest
    {
        public string SrcStockCode { get; set; }
        public string DesStockCode { get; set; }
        public string TransferType { get; set; }
        public string BatchCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }


    [Route("/api/v1/stock/transfer-card", "POST")]
    public class StockTransferCardRequest : IUserInfoRequest
    {
        public string SrcStockCode { get; set; }
        public string DesStockCode { get; set; }
        public string TransferType { get; set; }
        public string BatchCode { get; set; }
        public List<StockTransferItemInfoRespond> ProductList { get; set; }

        public string PartnerCode { get; set; }
        public string ParentCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }

    public class PayBatchItemDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public int Amount { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? Fee { get; set; }
        public int Quantity { get; set; }
        public string ReceiverInfo { get; set; }
        public string PartnerCode { get; set; }
        public string TransRef { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
    }

    [Route("/api/v1/backend/Offsettopup/history")]
    public class OffsetTopupRequest : PaggingBaseDto
    {

        public string OriginPartnerCode { get; set; }

        public string OriginTransCode { get; set; }

        public string TransCode { get; set; }

        public string PartnerCode { get; set; }

        public string ReceiverInfo { get; set; }

        public int Status { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }


    [Route("/api/v1/backend/OffsetTopup", "POST")]
    public class OffsetBuRequest
    {
        public string TransCode { get; set; }
    }
}
