using System.Threading.Tasks;

namespace HLS.Topup.SystemManagerment
{
    public interface ISystemManager
    {
        Task<bool> LockProvider(string providerCode, int timeLock = 30);
        Task<bool> UnLockProvider(string providerCode, bool isAuto = false);
    }
}
