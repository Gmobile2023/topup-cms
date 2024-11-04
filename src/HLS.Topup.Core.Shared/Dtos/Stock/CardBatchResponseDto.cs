using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HLS.Topup.Common;

namespace HLS.Topup.Dtos.Stock
{
    public class CardBatchResponseDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BatchCode { get; set; }
        public string BatchName { get; set; }
        public string Description { get; set; }
        public string StockType { get; set; }
        public CommonConst.CardPackageStatus Status { get; set; } 
        // public int? Quantity { get; set; }
        // public int? Value { get; set; } 
        [DataMember(Name = "provider")]
        public string ProviderCode { get; set; }
        
        // [DataMember(Name = "vendor")]
        // public string VendorCode { get; set; }
        [DataMember(Name = "quantity")]
        public int TotalQuantity { get; set; }
        [DataMember(Name = "amount")]
        public decimal TotalAmount { get; set; }
        public string ImportType { get; set; }
        public List<StockBatchItem> StockBatchItems { get; set; }
         
    }
    
    public class StockBatchItem 
    {
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }  
        /// <summary>
        /// chiết khấu khi nhập thẻ
        /// </summary>
        public float Discount { get; set; }
        public int ItemValue { get; set; }
        public string ProductCode { get; set; }
        /// <summary>
        /// Số lượng yêu cầu
        /// </summary>
        public int Quantity { get; set; } 
        public int QuantityImport { get; set; } 
        /// <summary>
        /// giá vốn
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Airtime nhập
        /// </summary>
        public decimal Airtime { get; set; }
    }
}