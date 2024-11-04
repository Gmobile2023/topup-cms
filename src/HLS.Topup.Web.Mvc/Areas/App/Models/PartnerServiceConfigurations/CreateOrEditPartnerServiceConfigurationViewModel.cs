using HLS.Topup.Configuration.PartnerServiceConfigurationDtos;
using System.Collections.Generic;

namespace HLS.Topup.Web.Areas.App.Models.PartnerServiceConfigurations
{
    public class CreateOrEditPartnerServiceConfigurationModalViewModel
    {
        public CreateOrEditPartnerServiceConfigurationDto ServiceConfiguration { get; set; }

        public string ServiceServicesName { get; set; }

        public string ProviderName { get; set; }

        public string CategoryCategoryName { get; set; }

        public string ProductProductName { get; set; }

        public string UserName { get; set; }


        public List<PartnerServiceConfigurationServiceLookupTableDto> PartnerServiceConfigurationServiceList { get; set; }

        public List<PartnerServiceConfigurationProviderLookupTableDto> PartnerServiceConfigurationProviderList { get; set; }

        public List<PartnerServiceConfigurationCategoryLookupTableDto> PartnerServiceConfigurationCategoryList { get; set; }


        public bool IsEditMode => ServiceConfiguration.Id.HasValue;

        public List<PartnerServiceConfigurationStatusResponseLookupTableDto> PartnerStatusResponseConfigurationCategoryList { get; set; }
    }
}
