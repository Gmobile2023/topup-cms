using System.Collections.Generic;
using HLS.Topup.Configuration.Dtos;

namespace HLS.Topup.Web.Areas.App.Models.ServiceConfigurations
{
    public class ServiceConfigurationsViewModel
    {
		public string FilterText { get; set; }
        public List<ServiceConfigurationServiceLookupTableDto> ServiceConfigurationServiceList { get; set;}

        public List<ServiceConfigurationProviderLookupTableDto> ServiceConfigurationProviderList { get; set;}

        public List<ServiceConfigurationCategoryLookupTableDto> ServiceConfigurationCategoryList { get; set;}
        public List<ServiceConfigurationProductLookupTableDto> ServiceConfigurationProductList { get; set; }
    }
}
