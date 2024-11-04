namespace HLS.Topup.Web.Models.TopupRequest
{
    public class TopupListItemDto
    {
        public string PhoneNumber { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public decimal CardPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal FixAmount { get; set; }
    }
}
