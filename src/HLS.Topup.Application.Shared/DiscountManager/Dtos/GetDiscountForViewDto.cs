using System;

namespace HLS.Topup.DiscountManager.Dtos
{
    public class GetDiscountForViewDto
    {
		public DiscountDto Discount { get; set; }
		public string UserName { get; set;}
		public string Approver { get; set;}
		public string Createtor { get; set;}
		public DateTime CreatedDate { get; set; }

    }
}