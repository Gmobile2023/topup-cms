namespace HLS.Topup.Configuration.PartnerServiceConfigurationDtos
{
    public class GetPartnerServiceConfigurationForEditOutput
    {
        public CreateOrEditPartnerServiceConfigurationDto ServiceConfiguration { get; set; }

        public string ServiceServicesName { get; set; }

        public string ProviderName { get; set; }

        public string CategoryCategoryName { get; set; }

        public string ProductProductName { get; set; }

        public string UserName { get; set; }
    }
}