using System.Collections.Generic;
using HLS.Topup.Editions.Dto;

namespace HLS.Topup.Web.Areas.App.Models.Tenants
{
    public class TenantIndexViewModel
    {
        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }
    }
}