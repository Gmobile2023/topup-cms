using System;

namespace HLS.Topup.LimitationManager.Dtos
{
    public class GetLimitProductForViewDto
    {
		public LimitProductDto LimitProduct { get; set; }

		public string UserName { get; set;}

		public string AgentName { get; set;}
		
		public string UserApproved { get; set;}
		
		public string UserCreated { get; set;}
		
		public DateTime CreationTime { get; set;}
    }
}