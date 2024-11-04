namespace HLS.Topup.PayBacks.Dtos
{
    public class PayBacksImportDto
    {
        public string AgentCode { get; set; }
        
        public long UserId { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string FullName { get; set; }
        
        public double Amount { get; set; }
        
        public bool IsActive { get; set; }
    }

    public class PayBacksImport
    {
        public string AgentCode { get; set; }
        
        public long UserId { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string FullName { get; set; }
        public double Amount { get; set; }
        
        public bool IsActive { get; set; }
    }
}