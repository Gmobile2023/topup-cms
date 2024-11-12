using System.Threading.Tasks;
using Abp.Runtime.Session;
using HLS.Topup.Sessions.Dto;
using HLS.Topup.Web.Session;
using Microsoft.AspNetCore.Mvc;

namespace HLS.Topup.Web.Views.Shared.Components.UserInfo
{
    public class UserInfoViewComponent : TopupViewComponent
    {
        private readonly IAbpSession _abpSession;
        private readonly IPerRequestSessionCache _sessionCache;

        public UserInfoViewComponent(IAbpSession abpSession,
            IPerRequestSessionCache sessionCache)
        {
            _abpSession = abpSession;
            _sessionCache = sessionCache;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(new UserInfoViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync(),
            });
        }
    }

    public class UserInfoViewModel
    {
        public GetCurrentLoginInformationsOutput LoginInformations { get; set; }
    }
}