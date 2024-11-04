using System.Threading.Tasks;
using HLS.Topup.Dtos.Cms;

namespace HLS.Topup.Cms
{
    public interface ICmsManager
    {
        Task<AcfAdvertiseAppDto> GetAdvertiseAcfByPage(int pageId);

        Task<AcfFaqsAppDto> GetFaqsAcfByPage(int pageId);
    }
}
