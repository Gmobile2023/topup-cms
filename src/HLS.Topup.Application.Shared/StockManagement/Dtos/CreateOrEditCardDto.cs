using HLS.Topup.Common;
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.StockManagement.Dtos
{
    public class CreateOrEditCardDto
    {
        public Guid? Id { get; set; }
        [Required]
        [StringLength(CardConsts.MaxSerialLength, MinimumLength = CardConsts.MinSerialLength)]
        public string Serial { get; set; }
        [Required]
        [StringLength(CardConsts.MaxCardCodeLength, MinimumLength = CardConsts.MinCardCodeLength)]
        public string CardCode { get; set; }
        public DateTime ExpiredDate { get; set; }
        public CommonConst.CardStatus Status { get; set; }
        public int CardValue { get; set; }
        public string BatchCode { get; set; }
        public string StockType { get; set; }
        public string StockCode { get; set; }
  
        // nha cung cap
        public string ProviderName{ get; set; }
        public string ProviderCode { get; set; }
        // dịch vu
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        // loai sp
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }
        
        public DateTime ImportedDate { get; set; }
        public DateTime ExportedDate { get; set; }
    }
}