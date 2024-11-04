using HLS.Topup.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Abp.Application.Services.Dto;

namespace HLS.Topup.StockManagement.Dtos
{
    public class CardBatchDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BatchCode { get; set; } 
        // public string BatchName { get; set; }
        public string Description { get; set; } 
        public CommonConst.CardPackageStatus Status { get; set; }  
        public string ProviderName{ get; set; }
        public string ProviderCode { get; set; }
        // public string VendorName { get; set; }
        // public string  VendorCode { get; set; } 
        public string ImportType { get; set; }
        public decimal TotalAmount
        {
            get
            {
                if (!StockBatchItems.Any())
                    return 0;
                return StockBatchItems.Sum(x => (x.QuantityImport * x.ItemValue) - (x.QuantityImport * x.ItemValue) * ((decimal)x.Discount/100) );
            }
        }
        public int TotalQuantity  {
            get
            {
                if (!StockBatchItems.Any())
                    return 0; 
                return StockBatchItems.Sum(x => (x.QuantityImport) );
            }
        }
        public List<StockBatchItem> StockBatchItems { get; set; }
    }
    
    public class StockBatchItem 
    {
        /// <summary>
        /// chiết khấu khi nhập thẻ
        /// </summary>
        public float Discount { get; set; }
        public int ItemValue { get; set; }
        public string ProductCode { get; set; } 
        public int Quantity { get; set; } 
        public int QuantityImport { get; set; } 
        /// <summary>
        /// giá vốn
        /// </summary>
        public decimal Amount { get; set; }
        
        public string ProductName { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
    }

    
    public class CardImportItem 
    {
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; } 
        public string CategoryCode { get; set; } 
        public string CategoryName { get; set; } 
        public string Serial { get; set; }
        public string CardCode { get; set; }
        public decimal CardValue { get; set; } 
        public DateTime ? ExpiredDate { get; set; }
        
        /// <summary>
        /// Can be set when reading data from excel or when importing user
        /// </summary>
        public string Exception { get; set; }

        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }
    
    public class CardImport
    {
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; } 
        public string CategoryCode { get; set; } 
        public string CategoryName { get; set; }  
        public string ProductCode { get; set; } 
        public string ProductName { get; set; }  
        public decimal CardValue { get; set; } 
        public int Quantity { get; set; }  
        public float Discount { get; set; }  
        public decimal Amount { get; set; }  
        public List<CardImportItem> Cards { get; set; } 
    }
    
}