
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Configuration.Dtos
{
    public class CreateOrEditServiceConfigurationDto : EntityDto<int?>
    {

        [Required]
        [StringLength(ServiceConfigurationConsts.MaxNameLength, MinimumLength = ServiceConfigurationConsts.MinNameLength)]
        public string Name { get; set; }


        public bool IsOpened { get; set; }


        public int Priority { get; set; }


        [StringLength(ServiceConfigurationConsts.MaxDescriptionLength, MinimumLength = ServiceConfigurationConsts.MinDescriptionLength)]
        public string Description { get; set; }


        public int? ServiceId { get; set; }

        public int? ProviderId { get; set; }

        public int? CategoryId { get; set; }

        public int? ProductId { get; set; }

        public long? UserId { get; set; }

        public int? ProviderSetTransactionTimeout { get; set; }

        public int? ProviderMaxWaitingTimeout { get; set; }
        
        public bool? IsEnableResponseWhenJustReceived { get; set; }
        [StringLength(ServiceConfigurationConsts.MaxStatusResponseWhenJustReceivedLength, MinimumLength = ServiceConfigurationConsts.MinStatusResponseWhenJustReceivedLength)]
        
        public string StatusResponseWhenJustReceived { get; set; }

        public int? WaitingTimeResponseWhenJustReceived { get; set; }
        public bool IsLastConfiguration { get; set; }
        public string AllowTopupReceiverType { get; set; }

        public decimal RateRunning { get; set; }
    }
}