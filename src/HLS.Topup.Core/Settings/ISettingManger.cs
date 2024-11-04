using System.Threading.Tasks;
using Abp;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Common;
using HLS.Topup.Dtos.Settings;

namespace HLS.Topup.Settings
{
    public interface ISettingManger
    {
        Task<bool> CheckAccountActivities(CheckAccountActivityInput input, long userId, string accountCode,
            UserIdentifier userIdentifier = null);

        Task<bool> CheckAccountActivitiesOld(CheckAccountActivityInput input, long userId, string accountCode);
        Task ChangePaymentVerifyMethod(PaymentMethodDto input, UserIdentifier userIdentifier);
        Task<CommonConst.VerifyTransType> GetPaymentVerifyMethod(CommonConst.Channel channel, UserIdentifier userIdentifier);
    }
}
