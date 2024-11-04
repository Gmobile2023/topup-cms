using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.Providers.Dtos
{
    public class GetAllProvidersInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string NameFilter { get; set; }

		public int? ProviderTypeFilter { get; set; }

		public int? ProviderStatusFilter { get; set; }
		public string ParentProviderFilter { get; set; }



    }
}