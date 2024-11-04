using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HLS.Topup.Common;
using ServiceStack.DataAnnotations;

namespace HLS.Topup.PayBacks.Dtos
{
    public class CreateOrEditPayBacksDto : EntityDto<int?>
    {
        public int? TenantId { get; set; }

        public int? CreatorUserId { get; set; }

        public DateTime? CreationTime { get; set; }

        [StringLength(50)] public string Code { get; set; }

        [Required] [StringLength(255)] public string Name { get; set; }

        [Required] public DateTime FromDate { get; set; }

        [Required] public DateTime ToDate { get; set; }

        public int? Total { get; set; }
        public decimal? TotalAmount { get; set; }

        public CommonConst.PayBackStatus Status { get; set; }
        public DateTime? DateApproved { get; set; }
        public DateTime? DatePay { get; set; }

        public long? ApproverId { get; set; }

        [StringLength(255)] public string Description { get; set; }

        public List<PayBacksItem> PayBacksDetail { get; set; }
    }

    public class PayBacksItem
    {
        public int? PayBackId { get; set; }
        public long UserId { get; set; }
        public decimal Amount { get; set; }
    }

    public class ApprovalPayBacksDto
    {
        public List<PaybatchAccount> Accounts { get; set; }
        public string CurrencyCode { get; set; }
        public string TransRef { get; set; }
        public string TransNote { get; set; }
    }

    public class PaybatchAccount
    {
        public string AccountCode { get; set; }
        public decimal Amount { get; set; }
        public bool Success { get; set; }
        public string TransRef { get; set; }
        public string TransNote { get; set; }
    }
}