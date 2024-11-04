using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Configuration.PartnerServiceConfigurationDtos
{
    public class CreateOrEditPartnerServiceConfigurationDto : EntityDto<int?>
    {
        [Required]
        [StringLength(ServiceConfigurationConsts.MaxNameLength, MinimumLength = ServiceConfigurationConsts.MinNameLength)]
        public string Name { get; set; }

        public byte Status { get; set; }

        [StringLength(ServiceConfigurationConsts.MaxDescriptionLength, MinimumLength = ServiceConfigurationConsts.MinDescriptionLength)]
        public string Description { get; set; }

        public int? ServiceId { get; set; }

        public int? ProviderId { get; set; }

        public int? CategoryId { get; set; }

        public long? UserId { get; set; }
    }
}