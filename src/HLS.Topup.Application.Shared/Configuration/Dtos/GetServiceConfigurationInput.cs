namespace HLS.Topup.Configuration.Dtos
{
    public class GetServiceConfigurationInput
    {
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public string AccountCode { get; set; }
        public bool IsCheckChannel { get; set; }
    }
}
