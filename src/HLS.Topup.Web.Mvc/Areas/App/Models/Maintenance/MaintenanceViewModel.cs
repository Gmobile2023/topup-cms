using System.Collections.Generic;
using HLS.Topup.Caching.Dto;

namespace HLS.Topup.Web.Areas.App.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}