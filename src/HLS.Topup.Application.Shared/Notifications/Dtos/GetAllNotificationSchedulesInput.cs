using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.Notifications.Dtos
{
    public class GetAllNotificationSchedulesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string NameFilter { get; set; }

		public int? StatusFilter { get; set; }

		public int? AccountTypeFilter { get; set; }

		public int? AgentTypeFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 
    }
}