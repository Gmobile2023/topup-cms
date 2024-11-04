using HLS.Topup.Common;
using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.StockManagement.Dtos
{
    public class CreateOrEditCardBatchDto
    {
        public Guid? Id { get; set; }
        [Required]
        [StringLength(CardBatchConsts.MaxBatchCodeLength, MinimumLength = CardBatchConsts.MinBatchCodeLength)]
        public string BatchCode { get; set; } 
        public DateTime CreatedDate { get; set; } 
        public CommonConst.CardPackageStatus Status { get; set; } 
        [StringLength(CardBatchConsts.MaxDescriptionLength, MinimumLength = CardBatchConsts.MinDescriptionLength)]
        public string Description { get; set; }


        public string ProviderCode { get; set; }
        public string ProviderName{ get; set; }
        
        public decimal TotalAmount { get; set; }
        public int TotalQuantity { get; set; }
        public List<StockBatchItem> StockBatchItems { get; set; }
    }
}