using Abp.AspNetCore.Mvc.Views;

namespace HLS.Topup.Web.Views
{
    public abstract class TopupRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected TopupRazorPage()
        {
            LocalizationSourceName = TopupConsts.LocalizationSourceName;
        }
    }
}
