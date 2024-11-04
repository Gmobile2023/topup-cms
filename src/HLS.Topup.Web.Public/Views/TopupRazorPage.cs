using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace HLS.Topup.Web.Public.Views
{
    public abstract class TopupRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected TopupRazorPage()
        {
            LocalizationSourceName = TopupConsts.LocalizationSourceName;
        }
    }
}
