using System;
using System.Collections.Generic;
using HLS.Topup.Common;
using HLS.Topup.Organizations.Dto;

namespace HLS.Topup.Authorization.Users.Dto
{
    public class GetUserForEditOutput
    {
        public Guid? ProfilePictureId { get; set; }

        public UserEditDto User { get; set; }
        public UserInfoDto ParentUser { get; set; }

        public UserRoleDto[] Roles { get; set; }

        public List<OrganizationUnitDto> AllOrganizationUnits { get; set; }

        public List<string> MemberedOrganizationUnits { get; set; }
    }


    public class AgentInfoDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
        public byte? Gender { get; set; }
        public DateTime? DoB { get; set; }
        public DateTime CreationTime { get; set; }
        public string AccountCode { get; set; }
    }
}
