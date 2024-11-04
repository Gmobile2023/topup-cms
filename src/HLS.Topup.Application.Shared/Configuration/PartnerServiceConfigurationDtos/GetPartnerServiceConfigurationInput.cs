namespace HLS.Topup.Configuration.PartnerServiceConfigurationDtos
{
    public class GetPartnerServiceConfigurationInput
    {
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public string AccountCode { get; set; }
        public bool IsCheckChannel { get; set; }
    }
}
