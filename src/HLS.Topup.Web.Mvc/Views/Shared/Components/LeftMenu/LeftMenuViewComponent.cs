﻿using Abp.Application.Navigation;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HLS.Topup.MultiTenancy;
using HLS.Topup.Web.Areas.App.Models.Layout;
using HLS.Topup.Web.Startup;

namespace HLS.Topup.Web.Views.Shared.Components.LeftMenu
{
    public class LeftMenuViewComponent : TopupViewComponent
    {
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly IAbpSession _abpSession;
        private readonly TenantManager _tenantManager;

        public LeftMenuViewComponent(
            IUserNavigationManager userNavigationManager,
            IAbpSession abpSession,
            TenantManager tenantManager)
        {
            _userNavigationManager = userNavigationManager;
            _abpSession = abpSession;
            _tenantManager = tenantManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(string currentPageName = null)
        {
            var model = new MenuViewModel
            {
                Menu = await _userNavigationManager.GetMenuAsync(LeftMenuNavigationProvider.MenuName, _abpSession.ToUserIdentifier()),
                CurrentPageName = currentPageName
            };
            return View("Default", model);
        }
    }
}
