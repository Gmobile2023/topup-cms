using System.Collections.Generic;
using HLS.Topup.Deposits.Dtos;

namespace HLS.Topup.Web.Areas.App.Models.Deposits
{
    public class DepositsViewModel
    {
		public string FilterText { get; set; }
        public List<DepositBankLookupTableDto> DepositBankList { get; set;}
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public long AgentId { get; set; }
        public string AgentInfo { get; set; }
        public long  SaleId { get; set; }
        public string SaleInfo { get; set; }
        public string Type { get; set; }
        public int Status { get; set; }
    }
}
