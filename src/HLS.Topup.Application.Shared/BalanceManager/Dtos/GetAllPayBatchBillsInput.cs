using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.BalanceManager.Dtos
{
    public class GetAllPayBatchBillsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CodeFilter { get; set; }

        public string NameFilter { get; set; }

        public int? StatusFilter { get; set; }

        public DateTime? MaxDateApprovedFilter { get; set; }
        public DateTime? MinDateApprovedFilter { get; set; }
        public string ProductCodeFilter { get; set; }
        public string CategoryCodeFilter { get; set; }

    }

    public class GetPayBatchSearchInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Số hóa đơn tối thiểu
        /// </summary>
        public int BlockMin { get; set; }

        /// <summary>
        /// Số tiền thưởng trên mỗi Block
        /// </summary>
        public decimal MoneyBlock { get; set; }

        /// <summary>
        /// Số tiền hóa đơn tối thiểu
        /// </summary>
        public decimal ? BillAmountMin { get; set; }

        /// <summary>
        /// Số tiền thưởng tối đa
        /// </summary>
        public decimal ? BonusMoneyMax { get; set; }
       

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string CategoryCode { get; set; }

        public string ProductCode { get; set; }

        public int IsSearch { get; set; }

    }

    public class GetPayBatchSearchDetailInput : PagedAndSortedResultRequestDto
    {
       public int Id { get; set; }

    }

    public class CheckPayBatchBillInput 
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
    }

}