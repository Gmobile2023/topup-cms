using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Sale.Dtos
{
    public class CreateOrEditSaleClearDebtDto : EntityDto<int?>
    {
        public virtual string TransCode { get; set; }
        public virtual CommonConst.ClearDebtStatus Status { get; set; }
        public decimal Amount { get; set; }
        public CommonConst.ClearDebtType Type { get; set; }

        [StringLength(SaleClearDebtConsts.MaxDescriptionsLength, MinimumLength = SaleClearDebtConsts.MinDescriptionsLength)]
        public string Descriptions { get; set; }
        public long UserId { get; set; }
        public int ? BankId { get; set; }

        public string TransCodeBank { get; set; }

    }
}
