using Abp.AutoMapper;
using Abp.Organizations;
using HLS.Topup.Authorization.Users;

namespace HLS.Topup.Authorization.Organization
{
    [AutoMap(typeof(OrganizationUnit), typeof(OrganizationsUnitCustom))]
    public class OrganizationsUnitCustom : OrganizationUnit
    {
        public virtual User User { get; set; }
        public virtual long? UserId { get; set; }
        public virtual byte? Status { get; set; }
    }
}
