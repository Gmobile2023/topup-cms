using GraphQL.Types;
using HLS.Topup.Dto;

namespace HLS.Topup.Types
{
    public class OrganizationUnitType : ObjectGraphType<OrganizationUnitDto>
    {
        public OrganizationUnitType()
        {
            Field(x => x.Id);
            Field(x => x.Code);
            Field(x => x.DisplayName);
            Field(x => x.TenantId, nullable: true);
        }
    }
}