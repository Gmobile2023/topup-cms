using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using HLS.Topup.Common;

namespace HLS.Topup.PostManagement.Dtos
{
    public class PostManagementDto : EntityDto<long?>
    {
        public bool IsEditMode { get; set; }
        public bool IsViewMode { get; set; }
        public string AccountCode { get; set; }
        [Required]

        public string PhoneNumber { get; set; }
        [Required]

        public string Surname { get; set; }
        [Required]

        public string Name { get; set; }

        public string FullName { get; set; }

        public CommonConst.AgentType? AgentType { get; set; }

        public DateTime? CreationTime { get; set; }

        public string CreatorName { get; set; }

        public bool IsActive { get; set; }

        public string Address { get; set; }

//        public string AddressView { get; set; }
        [Required]

        public string AgentName { get; set; }
        [Required]

        public int CityId { get; set; }
        [Required]

        public int DistrictId { get; set; }
        [Required]

        public int WardId { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
    }
}
