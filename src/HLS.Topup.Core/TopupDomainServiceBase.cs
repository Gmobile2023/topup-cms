using Abp.Domain.Services;

namespace HLS.Topup
{
    public abstract class TopupDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected TopupDomainServiceBase()
        {
            LocalizationSourceName = TopupConsts.LocalizationSourceName;
        }
    }
}
