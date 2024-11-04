using System;
using HLS.Topup.Common;

namespace HLS.Topup.Deposits.Dtos
{
    public class DepositRequestDto
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int BankId { get; set; }
        public string RequestCode { get; set; }
    }

    public class DepositRequestItemDto
    {
        public CommonConst.DepositStatus Status { get; set; }
        public string TransCode { get; set; }
        public string BankName { get; set; }
        public DateTime CreatetionTime { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string RequestCode { get; set; }
    }
}