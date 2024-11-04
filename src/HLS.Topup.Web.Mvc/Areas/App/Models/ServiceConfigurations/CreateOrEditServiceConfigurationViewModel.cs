using HLS.Topup.Configuration.Dtos;
using System.Collections.Generic;

namespace HLS.Topup.Web.Areas.App.Models.ServiceConfigurations
{
    public class CreateOrEditServiceConfigurationModalViewModel
    {
        public CreateOrEditServiceConfigurationDto ServiceConfiguration { get; set; }

        public string ServiceServicesName { get; set; }

        public string ProviderName { get; set; }

        public string CategoryCategoryName { get; set; }

        public string ProductProductName { get; set; }

        public string UserName { get; set; }

        public bool IsDispalyRate { get; set; }
        public List<ServiceConfigurationServiceLookupTableDto> ServiceConfigurationServiceList { get; set; }

        public List<ServiceConfigurationProviderLookupTableDto> ServiceConfigurationProviderList { get; set; }

        public List<ServiceConfigurationCategoryLookupTableDto> ServiceConfigurationCategoryList { get; set; }


        public bool IsEditMode => ServiceConfiguration.Id.HasValue;

        public List<ServiceConfigurationStatusResponseLookupTableDto> StatusResponseConfigurationCategoryList { get; set; }
    }
}
