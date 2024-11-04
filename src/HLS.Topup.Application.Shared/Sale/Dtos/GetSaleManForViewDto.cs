namespace HLS.Topup.Sale.Dtos
{
    public class GetSaleManForViewDto
    {
		public SaleManDto SaleMan { get; set; }
        
        public string SaleLeadName { get; set; }
        
        public string CreatorName { get; set; }
        
        public long? SaleLeadId { get; set; }
    }
}
