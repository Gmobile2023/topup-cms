using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace HLS.Topup.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
