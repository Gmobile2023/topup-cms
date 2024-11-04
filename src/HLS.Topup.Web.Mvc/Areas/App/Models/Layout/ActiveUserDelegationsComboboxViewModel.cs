using System.Collections.Generic;
using HLS.Topup.Authorization.Delegation;
using HLS.Topup.Authorization.Users.Delegation.Dto;

namespace HLS.Topup.Web.Areas.App.Models.Layout
{
    public class ActiveUserDelegationsComboboxViewModel
    {
        public IUserDelegationConfiguration UserDelegationConfiguration { get; set; }
        
        public List<UserDelegationDto> UserDelegations { get; set; }
    }
}
