using System.Collections.Generic;
using HLS.Topup.Providers.Dtos;
using HLS.Topup.Dtos.Provider;
using HLS.Topup.StockManagement.Dtos;

namespace HLS.Topup.Web.Areas.App.Models.Providers
{
    public class CreateOrEditProviderModalViewModel
    {
        public CreateOrEditProviderDto Provider { get; set; }

        public ProviderUpdateInfo ProviderUpdate { get; set; }

        public bool IsEditMode => Provider.Id.HasValue;
        public List<CommonLookupTableDto> ProviderList { get; set; }

        public bool IsDisplayRate { get; set; }
    }
}
