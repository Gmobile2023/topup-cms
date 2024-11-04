using System.Collections.Generic;
using MvvmHelpers;
using HLS.Topup.Models.NavigationMenu;

namespace HLS.Topup.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}