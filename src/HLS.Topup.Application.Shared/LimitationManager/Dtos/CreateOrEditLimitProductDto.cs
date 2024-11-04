using HLS.Topup.Common;
using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.LimitationManager.Dtos
{
    public class CreateOrEditLimitProductDto : EntityDto<int?>
    {
        public string Code { get; set; }
        
        [Required]
        [StringLength(LimitProductConsts.MaxNameLength, MinimumLength = LimitProductConsts.MinNameLength)]
        public string Name { get; set; }

        public DateTime FromDate { get; set; }
        
        public DateTime ToDate { get; set; }

        public DateTime? DateApproved { get; set; }

        public CommonConst.AgentType AgentType { get; set; }

        [StringLength(LimitProductConsts.MaxDescriptionLength, MinimumLength = LimitProductConsts.MinDescriptionLength)]
        public string Description { get; set; }

        public CommonConst.LimitProductConfigStatus Status { get; set; }
        
        public long? UserId { get; set; }
        
        public List<long?> ListUserId { get; set; }
        
        public long? CreatorUserId { get; set; }
        
        public long? ApproverId { get; set; }
        
        public List<LimitProduct> LimitProductsDetail { get; set; }
    }

    public class LimitProduct
    {
        public int LimitProductId { get; set; }
        
        public int? LimitQuantity { get; set; }
        
        public decimal? LimitAmount { get; set; }
        
        public int ProductId { get; set; }
    }
}