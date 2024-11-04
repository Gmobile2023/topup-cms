using HLS.Topup.Common;
using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ServiceStack;

namespace HLS.Topup.FeeManager.Dtos
{
    public class CreateOrEditFeeDto : EntityDto<int?>
    {
        [StringLength(FeeConsts.MaxCodeLength, MinimumLength = FeeConsts.MinCodeLength)]
        public string Code { get; set; }
        
        [Required]
        [StringLength(FeeConsts.MaxNameLength, MinimumLength = FeeConsts.MinNameLength)]
        public string Name { get; set; }

        public DateTime FromDate { get; set; }
        
        public DateTime ToDate { get; set; }

        public DateTime? DateApproved { get; set; }

        public CommonConst.FeeStatus Status { get; set; }

        public CommonConst.AgentType AgentType { get; set; }

        [StringLength(FeeConsts.MaxDescriptionLength, MinimumLength = FeeConsts.MinDescriptionLength)]
        public string Description { get; set; }

        public string ProductType { get; set; }
        
        public string ProductList { get; set; }

        public List<int> ProductTypeList
        {
            get
            {
                return ProductType.FromJson<List<int>>();
            }
        }
        
        public List<int> ProductListList
        {
            get
            {
                return ProductList.FromJson<List<int>>();
            }
        }

        public long? UserId { get; set; }
        
        public List<long?> ListUserId { get; set; }

        public List<ProductFeeItem> FeeDetail { get; set; }
    }

    public class ProductFeeItem
    {
        public int ProductId { get; set; }
        public decimal? MinFee { get; set; }
        public decimal? AmountMinFee { get; set; }
        public decimal? AmountIncrease { get; set; }
        public decimal? SubFee { get; set; }
    }
}