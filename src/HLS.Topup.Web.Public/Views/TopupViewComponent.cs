using Abp.AspNetCore.Mvc.ViewComponents;

namespace HLS.Topup.Web.Public.Views
{
    public abstract class TopupViewComponent : AbpViewComponent
    {
        protected TopupViewComponent()
        {
            LocalizationSourceName = TopupConsts.LocalizationSourceName;
        }
    }
}