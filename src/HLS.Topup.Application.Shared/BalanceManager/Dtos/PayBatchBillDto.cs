using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.BalanceManager.Dtos
{
    public class PayBatchBillDto : EntityDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public CommonConst.PayBatchBillStatus Status { get; set; }

        public int TotalAgent { get; set; }

        public int TotalTrans { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Period { get; set; }

        public int TotalBlockBill { get; set; }

        public decimal AmountPayBlock { get; set; }

        public long? ApproverId { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime? DateApproved { get; set; }

        public DateTime CreationTime { get; set; }

        public string Description { get; set; }

        public decimal? MinBillAmount { get; set; }

        public decimal? MaxAmountPay { get; set; }

        public int ProductId { get; set; }
        public int CategoryId { get; set; }


    }
}
