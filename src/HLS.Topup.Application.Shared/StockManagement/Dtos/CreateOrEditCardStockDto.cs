using HLS.Topup.Common;

using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.StockManagement.Dtos
{
    public class CreateOrEditCardStockDto
    {
	    public Guid Id { get; set; }

		[Required]
		[StringLength(CardStockConsts.MaxStockCodeLength, MinimumLength = CardStockConsts.MinStockCodeLength)]
		public string StockCode { get; set; }
		
		
		public int CardValue { get; set; }
		
		
		public int Inventory { get; set; }
		
		
		public int InventoryLimit { get; set; }
		
		
		public int MinimumInventoryLimit { get; set; }
		
		
		public CommonConst.CardStockStatus Status { get; set; }
		
		
		[StringLength(CardStockConsts.MaxProductCodeLength, MinimumLength = CardStockConsts.MinProductCodeLength)]
		public string ProductCode { get; set; }
		public string KeyCode { get; set; }
		
		[StringLength(CardStockConsts.MaxDescriptionLength, MinimumLength = CardStockConsts.MinDescriptionLength)]
		public string Description { get; set; } 
		 public string ActionType { get; set; }
	  
		 public string ProductName { get; set; } 
		 // dịch vu
		 public string ServiceName { get; set; }
		 public string ServiceCode { get; set; }
		 // loai sp
		 public string CategoryName { get; set; }
		 public string CategoryCode { get; set; }
		 
    }
    public class TransferCardStockDto
    {
	    public Guid Id { get; set; }  
	    public DateTime CreatedDate { get; set; }
	    public string SrcStockCode { get; set; }
	    public string DesStockCode { get; set; } 
	    public string VendorCode { get; set; } 
	    public string BatchCode { get; set; } 
	    public int? CardValue { get; set; }
	    public int? Quantity { get; set; }
	    public string Description { get; set; } 
	    public byte Status { get; set; } 
    }
    
    
    public class GetCardInfoTransferInput
    { 
	    public string SrcStockCode { get; set; }
	    public string DesStockCode { get; set; }
	    public string TransferType { get; set; }
	    public string BatchCode { get; set; }
	    public string CategoryCode { get; set; }  
	    public string ProductCode { get; set; }  
    }
    
    public class StockTransferItemInfo
    {
	    public string ServiceCode { get; set; }
	    public string ServiceName { get; set; }
	    public string CategoryCode { get; set; }
	    public string CategoryName { get; set; }
	    public string ProductCode { get; set; }
	    public string ProductName { get; set; }
	    public decimal CardValue { get; set; }
	    public int QuantityAvailable { get; set; }
	    public int Quantity { get; set; }
    }
    
    
    public class StockTransferInput
    { 
	    public string SrcStockCode { get; set; }
	    public string DesStockCode { get; set; }
	    public string TransferType { get; set; }
	    public string BatchCode { get; set; }   
	    public List<StockTransferItemInfo> ProductList { get; set; }  
    }

    public class EditQuantityStockDto
    {               
        public string StockCode { get; set; }      
        public int Inventory { get; set; }      
        public string KeyCode { get; set; }       
    }
}