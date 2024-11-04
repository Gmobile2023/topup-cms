using HLS.Topup.Common;

namespace HLS.Topup.Deposits.Dtos
{
    public class GetDepositForViewDto
    {
		public DepositDto Deposit { get; set; }

		public string UserName { get; set; }
		
		public int? BankId { get; set; }

		public string BankBankName { get; set; }

		public string UserName2 { get; set; }

		public CommonConst.AgentType AgentType { get; set; }
		
		public string AgentName { get; set; }
		
		public string SaleLeader { get; set; }
		
		public string SaleMan { get; set; }

		public string CreatorName { get; set; }
    }
}