﻿using System.Collections.Generic;
using Abp.Application.Navigation;
using Abp.Extensions;
using Abp.Localization;
using HLS.Topup.Sessions.Dto;

namespace HLS.Topup.Web.Views.Shared.Components.Header
{
    public class HeaderViewModel
    {
        public GetCurrentLoginInformationsOutput LoginInformations { get; set; }

        public IReadOnlyList<LanguageInfo> Languages { get; set; }

        public LanguageInfo CurrentLanguage { get; set; }

        public UserMenu Menu { get; set; }

        public string CurrentPageName { get; set; }

        public bool IsMultiTenancyEnabled { get; set; }

        public bool TenantRegistrationEnabled { get; set; }

        public bool IsInHostView { get; set; }

        public string AdminWebSiteRootAddress { get; set; }

        public string WebSiteRootAddress { get; set; }

        public string GetShownLoginName()
        {
            if (!IsMultiTenancyEnabled)
            {
                return LoginInformations.User.UserName;
            }

            return LoginInformations.Tenant == null
                ? ".\\" + LoginInformations.User.UserName
                : LoginInformations.Tenant.TenancyName + "\\" + LoginInformations.User.UserName;
        }

        public string GetLogoUrl(string appPath)
        {
            if (!IsMultiTenancyEnabled || LoginInformations?.Tenant?.LogoId == null)
            {
                return appPath + "Common/Images/app-logo-on-light.svg";
            }

            return AdminWebSiteRootAddress.EnsureEndsWith('/') + "TenantCustomization/GetLogo?id=" + LoginInformations.Tenant.LogoId;
        }
    }

    public class CustomMenuItem
    {
        //
        // Summary:
        //     Creates a new Abp.Application.Navigation.UserMenuItem object.


        //
        // Summary:
        //     Unique name of the menu item in the application.
        public int? CommonMenuId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        //
        // Summary:
        //     Icon of the menu item if exists.
        public string Icon { get; set; }
        //
        // Summary:
        //     Display name of the menu item.
        public string DisplayName { get; set; }
        //
        // Summary:
        //     The Display order of the menu. Optional.
        public int Order { get; set; }
        //
        // Summary:
        //     The URL to navigate when this menu item is selected.
        public string Url { get; set; }
        //
        // Summary:
        //     A custom object related to this menu item.
        public object CustomData { get; set; }
        //
        // Summary:
        //     Target of the menu item. Can be "_blank", "_self", "_parent", "_top" or a frame
        //     name.
        public string Target { get; set; }
        //
        // Summary:
        //     Can be used to enable/disable a menu item.
        public bool IsEnabled { get; set; }
        //
        // Summary:
        //     Can be used to show/hide a menu item.
        public bool IsVisible { get; set; }
        //
        // Summary:
        //     Sub items of this menu item.
        public List<CustomMenuItem> Items { get; set; }
    }
}