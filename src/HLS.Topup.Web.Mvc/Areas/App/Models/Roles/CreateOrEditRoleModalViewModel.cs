using Abp.AutoMapper;
using HLS.Topup.Authorization.Roles.Dto;
using HLS.Topup.Web.Areas.App.Models.Common;

namespace HLS.Topup.Web.Areas.App.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode => Role.Id.HasValue;
    }
}