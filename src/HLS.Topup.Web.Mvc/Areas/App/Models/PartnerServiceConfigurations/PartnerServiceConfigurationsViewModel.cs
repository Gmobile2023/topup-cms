using System.Collections.Generic;
using HLS.Topup.Configuration.PartnerServiceConfigurationDtos;

namespace HLS.Topup.Web.Areas.App.Models.PartnerServiceConfigurations
{
    public class PartnerServiceConfigurationsViewModel
    {
		public string FilterText { get; set; }
        public List<PartnerServiceConfigurationServiceLookupTableDto> PartnerServiceConfigurationServiceList { get; set;}

        public List<PartnerServiceConfigurationProviderLookupTableDto> PartnerServiceConfigurationProviderList { get; set;}

        public List<PartnerServiceConfigurationCategoryLookupTableDto> PartnerServiceConfigurationCategoryList { get; set;}
        public List<PartnerServiceConfigurationProductLookupTableDto> PartnerServiceConfigurationProductList { get; set; }
    }
}
