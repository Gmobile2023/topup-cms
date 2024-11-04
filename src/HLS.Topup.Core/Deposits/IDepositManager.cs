using System.Threading.Tasks;

namespace HLS.Topup.Deposits
{
    public interface IDepositManager
    {
        Task ApprovalDeposit(string transcode, string transCodeBank, string description, long? approvalId);
    }
}
