using System.Threading.Tasks;
using HLS.Topup.Authorization.Users;

namespace HLS.Topup.AccountManager
{
    public interface IAccountManager
    {
        Task<User> CreateUserAsync(CreateAccountDto input);
        Task<User> UpdateUserAsync(CreateAccountDto input);
        Task<UserProfileDto> GetAccount(long userId);
        Task<UserProfileDto> GetAccountByCode(string accountCode);
    }
}
