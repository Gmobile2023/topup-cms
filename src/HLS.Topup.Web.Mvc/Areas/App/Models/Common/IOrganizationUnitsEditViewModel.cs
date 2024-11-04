using System.Collections.Generic;
using HLS.Topup.Organizations.Dto;

namespace HLS.Topup.Web.Areas.App.Models.Common
{
    public interface IOrganizationUnitsEditViewModel
    {
        List<OrganizationUnitDto> AllOrganizationUnits { get; set; }

        List<string> MemberedOrganizationUnits { get; set; }
    }
}