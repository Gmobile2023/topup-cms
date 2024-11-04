using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using HLS.Topup.Cms.Dots;

namespace HLS.Topup.Cms
{
    public interface ICmsAppService:IApplicationService
    {
        Task<List<AdvertiseItemsDto>> GetAdvertiseItems(string position);

        Task<List<FaqsDto>> GetFaqsItems();
    }
}
