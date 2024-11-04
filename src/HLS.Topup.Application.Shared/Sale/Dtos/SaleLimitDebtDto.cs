using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Sale.Dtos
{
    public class SaleLimitDebtDto : EntityDto
    {
        public string SaleInfo { get; set; }

        public string SaleLeaderInfo { get; set; }

        public decimal LimitAmount { get; set; }

        public int DebtAge { get; set; }

        public CommonConst.DebtLimitAmountStatus Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UserCreated { get; set; }

        public string Description { get; set; }

        public long UserId { get; set; }
    }
}
