using System.Threading.Tasks;
using HLS.Topup.Dtos.Configuration;

namespace HLS.Topup.Configuration
{
    public interface IStaffConfigurationManager
    {
        Task CreateUserStaff(CreateStaffUserInput input);
        Task<bool> CheckStaffAction(long userId);
        Task UpdateUserStaff(UpdateStaffUserInput input);

        Task<UserStaffDto> GetUserStaffInfo(long userId);

    }
}
