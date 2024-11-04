using System.Threading.Tasks;
using HLS.Topup.Sessions.Dto;

namespace HLS.Topup.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
