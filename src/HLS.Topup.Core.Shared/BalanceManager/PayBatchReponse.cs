using System;
using System.Collections.Generic;
using System.Text;

namespace HLS.Topup.BalanceManager
{
    public class PayBatchBillItem
    {
        public long UserId { get; set; }
        public string AgentCode { get; set; }

        public string Mobile { get; set; }

        public string FullName { get; set; }

        public int Quantity { get; set; }

        public decimal PayAmount { get; set; }

        public string StatusName { get; set; }

        public decimal PayBatchMoney { get; set; }
        public string TransRef { get; set; }
    }
}
