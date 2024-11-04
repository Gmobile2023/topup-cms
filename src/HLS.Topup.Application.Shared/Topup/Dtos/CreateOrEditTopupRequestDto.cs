using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HLS.Topup.Common;
using HLS.Topup.Products.Dtos;
using ServiceStack.DataAnnotations;

namespace HLS.Topup.Topup.Dtos
{
    public class CreateOrEditTopupRequestDto : EntityDto<int?>
    {
        [Required]
        [StringLength(TopupRequestConsts.MaxMobileNumberLength,
            MinimumLength = TopupRequestConsts.MinMobileNumberLength)]
        public string PhoneNumber { get; set; }
        public string TransCode { get; set; }

         public string ServiceCode { get; set; }
        [Required] public int Amount { get; set; }
        [Required] public string ProductCode { get; set; }
        [Required] public string CategoryCode { get; set; }
        public CommonConst.Channel Channel { get; set; }
        public string PartnerCode { get; set; }
    }

    public class CreateOrEditTopupListRequestDto : EntityDto<int?>
    {
        [Required] public string CategoryCode { get; set; }

        [Required] public string BatchType { get; set; }
        [Required] public List<ImportTopupDto> ListNumbers { get; set; }
    }
    public class ImportTopupDto
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public int Quantity { get; set; }
        public decimal CardPrice { get; set; }
        public decimal value { get; set; }
        public decimal Discount { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }      
    }


    public class CreateOrEditPinCodeRequestDto : EntityDto<int?>
    {
        public int Amount { get; set; }
        [Required] public string CategoryCode { get; set; }
        public string Email { get; set; }
        [Required] public string ProductCode { get; set; }
        [Required] public string ServiceCode { get; set; }
        public int Quantity { get; set; }
        public CommonConst.Channel Channel { get; set; }
        public string TransCode { get; set; }
        public string PartnerCode { get; set; }
    }


    public class CancelTopupDto
    {
        public string TransCode { get; set; }
    }

    public class RefundTransDto
    {
        public string TransCode { get; set; }
        public string TransRef { get; set; }
        public string PartnerCode { get; set; }
        public decimal PaymentAmount { get; set; }
    }

    public class PriorityTopupDto
    {
        public string TransCode { get; set; }
        public decimal DiscountPriority { get; set; }
    }


    public class TopupPriceDto : ProductDto
    {
        public decimal Discount { get; set; }
        public decimal ProductValueDiscount { get; set; }
    }

    public class BatchLotStopInput
    {
        [Required]
        public string BatchCode { get; set; }
    }
}
