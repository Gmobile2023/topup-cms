using HLS.Topup.Common;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Text;

namespace HLS.Topup.BalanceManager
{

    [Route("/api/v1/backend/getPayBatchBill", "GET")]
    public class PayBatchBillRequest : PaggingBaseDto
    {
        /// <summary>
        /// Số hóa đơn tối thiểu
        /// </summary>
        public int BlockMin { get; set; }
        /// <summary>
        /// Số tiền hóa đơn tối thiểu
        /// </summary>
        public decimal BillAmountMin { get; set; }

        /// <summary>
        /// Số tiền thưởng tối đa
        /// </summary>
        public decimal BonusMoneyMax { get; set; }
        /// <summary>
        /// Số tiền thưởng trên mỗi Block
        /// </summary>
        public decimal MoneyBlock { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string CategoryCode { get; set; }

        public string ProductCode { get; set; }
    }
}
