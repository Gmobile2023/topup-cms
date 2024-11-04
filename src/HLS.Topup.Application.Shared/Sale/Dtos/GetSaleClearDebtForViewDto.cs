namespace HLS.Topup.Sale.Dtos
{
    public class GetSaleClearDebtForViewDto
    {
		public SaleClearDebtDto SaleClearDebt { get; set; }

		public string UserName { get; set;}

		public string BankBankName { get; set;}

        public string StatusName { get; set; }

    }
}