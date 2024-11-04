using Abp.Application.Services.Dto;

namespace HLS.Topup.Sale.Dtos
{
    public class SaleLimitDebtUserLookupTableDto
    {
		public long Id { get; set; }
		public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
    }
}