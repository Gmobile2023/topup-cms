using Abp.Application.Services.Dto;

namespace HLS.Topup.Deposits.Dtos
{
    public class DepositUserLookupTableDto
    {
		public long Id { get; set; }

		public string DisplayName { get; set; }
		
		public  string PhoneNumber { get; set; }
		public  string UserName { get; set; }
    }
}