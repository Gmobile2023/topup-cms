using Abp.AutoMapper;
using HLS.Topup.Organizations.Dto;

namespace HLS.Topup.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}