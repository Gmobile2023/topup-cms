using HLS.Topup.Common;
using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Dtos.Discounts;

namespace HLS.Topup.DiscountManager.Dtos
{
    public class CreateOrEditDiscountDto : EntityDto<int?>
    {
        [StringLength(DiscountConsts.MaxCodeLength, MinimumLength = DiscountConsts.MinCodeLength)]
        public string Code { get; set; }


        [Required]
        [StringLength(DiscountConsts.MaxNameLength, MinimumLength = DiscountConsts.MinNameLength)]
        public string Name { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime? DateApproved { get; set; }
        public CommonConst.DiscountStatus Status { get; set; }
        public CommonConst.DiscountType DiscountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }

        [StringLength(DiscountConsts.MaxDesciptionsLength, MinimumLength = DiscountConsts.MinDesciptionsLength)]
        public string Desciptions { get; set; }

        public long? UserId { get; set; }
        public List<long?> ListUserId { get; set; }
        public long? ApproverId { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? CreationTime { get; set; }
        public List<ProductDiscountItem> DiscountDetail { get; set; }
    }

    public class ProductDiscountItem
    {
        public int ProductId { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? FixAmount { get; set; }
    }
}