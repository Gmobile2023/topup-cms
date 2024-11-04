using HLS.Topup.Common;
using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.StockManagement.Dtos
{
    public class CardApiImportDto
    {
        public Guid? Id { get; set; }               
        public string ProviderCode { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public List<CardApiImportItemDto> CardItems { get; set; } 
    }

    public class CardApiImportItemDto
    {
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }  
        public string ProductCode { get; set; }   
        public decimal CardValue { get; set; }
        public int Quantity { get; set; }
        public float Discount { get; set; }  
    }
}