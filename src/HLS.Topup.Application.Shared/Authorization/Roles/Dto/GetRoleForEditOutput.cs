using System.Collections.Generic;
using HLS.Topup.Authorization.Permissions.Dto;

namespace HLS.Topup.Authorization.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}