
using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Configuration.Dtos
{
    public class ServiceConfigurationDto : EntityDto
    {
        public string Name { get; set; }

        public bool IsOpened { get; set; }

        public int Priority { get; set; }


        public int? ServiceId { get; set; }

        public int? ProviderId { get; set; }

        public int? CategoryId { get; set; }

        public int? ProductId { get; set; }

        public long? UserId { get; set; }

        public int? ProviderSetTransactionTimeout { get; set; }
        
        public int? ProviderMaxWaitingTimeout { get; set; }
        
        public bool? IsEnableResponseWhenJustReceived { get; set; }
        
        public string StatusResponseWhenJustReceived { get; set; }

        public int? WaitingTimeResponseWhenJustReceived { get; set; }
    }
}