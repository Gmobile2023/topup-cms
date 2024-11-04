using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Sale.Dtos
{
    public class SaleClearDebtDto : EntityDto
    {

        public virtual string TransCode { get; set; }
        public virtual CommonConst.ClearDebtStatus Status { get; set; }
        public decimal Amount { get; set; }
        public CommonConst.ClearDebtType Type { get; set; }

        public string SaleInfo { get; set; }
        public string Descriptions { get; set; }
        public long UserId { get; set; }
        public int BankId { get; set; }        
        public DateTime CreationTime { get; set; }
        public string UserCreated { get; set; }
        public string UserModify { get; set; }
        public DateTime ? ModifyDate { get; set; }
        public string ApprovalNote { get; set; }
        public string TransCodeBank { get; set; }
    }
}
