using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HLS.Topup.Common;
using HLS.Topup.Products.Dtos;
using ServiceStack.DataAnnotations;

namespace HLS.Topup.Topup.Dtos
{

    public class TopupListRequestDto : EntityDto<int?>
    {
      
        [Required] public string BatchType { get; set; }
        [Required] public List<ImportBatchDto> ListNumbers { get; set; }
    }
    public class ImportBatchDto
    {
        public string Id { get; set; }
        public string ReceiverInfo { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Fee { get; set; }
        public decimal Value { get; set; }
        public decimal Discount { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Decription { get; set; }
    }
}
