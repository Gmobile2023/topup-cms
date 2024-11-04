using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.PayBacks.Dtos;

namespace HLS.Topup.Paybacks
{
    public interface IPayBacksManager
    {
        Task<List<PayBacksDetailDto>> GetPayBacksDetails(int payBacksId);
    }
}