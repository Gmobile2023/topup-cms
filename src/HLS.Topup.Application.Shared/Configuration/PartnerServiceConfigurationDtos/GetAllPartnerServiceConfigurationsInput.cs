using Abp.Application.Services.Dto;
using HLS.Topup.Common;
using System;
using System.Collections.Generic;

namespace HLS.Topup.Configuration.PartnerServiceConfigurationDtos
{
    public class GetAllPartnerServiceConfigurationsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public int? StatusFilter { get; set; }

        public int? MaxPriorityFilter { get; set; }
        public int? MinPriorityFilter { get; set; }

        public string ServiceServicesNameFilter { get; set; }
        public int? ServiceId { get; set; }
        public int? ProviderId { get; set; }
        public int? CategoryId { get; set; }
        public int? ProductId { get; set; }

        public List<int> ServiceIds { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<int> ProductIds { get; set; }
        public long? UserId { get; set; }

        public string ProviderNameFilter { get; set; }

        public string CategoryCategoryNameFilter { get; set; }

        public string ProductProductNameFilter { get; set; }

        public string UserNameFilter { get; set; }
    }
}
