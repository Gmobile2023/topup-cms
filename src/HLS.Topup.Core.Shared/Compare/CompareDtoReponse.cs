using System;
using System.Collections.Generic;
using System.Text;

namespace HLS.Topup.Compare
{
    public class CompareDtoReponse
    {
        /// <summary>
        /// Ngày đối soát
        /// </summary>
        public DateTime CompareDate { get; set; }

        /// <summary>
        /// Ngày giao dịch
        /// </summary>
        public DateTime TransDate { get; set; }

        /// <summary>
        /// Nhà cung cấp
        /// </summary>
        public string ProviderCode { get; set; }

        /// <summary>
        /// Tên file đối soát của Nhất Trần
        /// </summary>
        public string SysFileName { get; set; }

        /// <summary>
        /// Tên file đối soát của nhà cung cấp
        /// </summary>
        public string ProviderFileName { get; set; }

        /// <summary>
        /// Số lượng giao dịch của hệ thống Nhất Trần
        /// </summary>
        public int SysQuantity { get; set; }


        /// <summary>
        /// Tổng số tiền giao dịch
        /// </summary>
        public decimal SysAmount { get; set; }

        /// <summary>
        /// Số giao dịch của nhà cung cấp
        /// </summary>
        public int ProviderQuantity { get; set; }

        /// <summary>
        /// Số tiền của nhà cung cấp
        /// </summary>
        public decimal ProviderAmount { get; set; }

        /// <summary>
        /// Số giao dịch khớp
        /// </summary>
        public int SameQuantity { get; set; }

        /// <summary>
        /// Số tiền khớp
        /// </summary>
        public decimal SameAmount { get; set; }

        /// <summary>
        /// Số giao dịch nhà cung cấp có mà NT không có
        /// </summary>
        public int ProviderOnlyQuantity { get; set; }

        /// <summary>
        /// Số tiền giao dịch nhà cung cấp có mà NT không có
        /// </summary>
        public decimal ProviderOnlyAmount { get; set; }

        /// <summary>
        /// Số giao dịch NT có NCC không có
        /// </summary>
        public int SysOnlyQuantity { get; set; }

        /// <summary>
        /// Số tiền giao dịch NT có mà NCC không có
        /// </summary>
        public decimal SysOnlyAmount { get; set; }

        /// <summary>
        /// Số giao dịch lệch
        /// </summary>
        public int NotSameQuantity { get; set; }

        /// <summary>
        /// Số tiền lệch
        /// </summary>
        public decimal NotSameAmount { get; set; }

        public bool Isenabled { get; set; }

        /// <summary>
        /// Người đối soát
        /// </summary>
        public string AccountCompare { get; set; }

        public string KeyCode { get; set; }
    }

    public class CompareReponseDto
    {
        public string CompareType { get; set; }

        public int Quantity { get; set; }

        public decimal AmountSys { get; set; }

        public decimal AmountProvider { get; set; }

        public decimal Deviation { get; set; }
    }

    public class CompareRefunDto
    {
        public DateTime TransDate { get; set; }

        public string TransDateSoft { get; set; }

        public string Provider { get; set; }

        public int Quantity { get; set; }

        public decimal Amount { get; set; }

        public int RefundWaitQuantity { get; set; }

        public decimal RefundWaitAmount { get; set; }

        public int RefundQuantity { get; set; }

        public decimal RefundAmount { get; set; }

        public string KeyCode { get; set; }
    }

    public class CompareRefunDetailDto
    {
        public DateTime TransDate { get; set; }

        public string AgentCode { get; set; }

        public string TransCode { get; set; }

        public string TransPay { get; set; }

        public decimal ProductValue { get; set; }

        public decimal Amount { get; set; }

        public string ReceivedAccount { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }

        public string TransCodeRefund { get; set; }

    }

    public class CompareReponseDetailDto
    {
        public DateTime TransDate { get; set; }

        public string AgentCode { get; set; }

        public string TransCode { get; set; }

        public string TransPay { get; set; }

        public decimal ProductValue { get; set; }

        public decimal Amount { get; set; }

        public string ReceivedAccount { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
    }

    public class CompareFileDto
    {
        public string CompareType { get; set; }

        public int Quantity { get; set; }

        public decimal Amount { get; set; }

        public int QuantityRefund { get; set; }

        public decimal AmountRefund { get; set; }
    }
}
