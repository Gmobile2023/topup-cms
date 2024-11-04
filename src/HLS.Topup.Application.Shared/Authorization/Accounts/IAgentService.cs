using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization.Accounts.Dto;
using HLS.Topup.Authorization.Users.Dto;
using HLS.Topup.DiscountManager.Dtos;
using HLS.Topup.Dtos.Configuration;
using HLS.Topup.Dtos.Discounts;
using HLS.Topup.Dtos.Fees;
using HLS.Topup.FeeManager.Dtos;
using HLS.Topup.LimitationManager.Dtos;

namespace HLS.Topup.Authorization.Accounts
{
    public interface IAgentService : IApplicationService
    {
        Task<AgentInfoDto> CreateAgent(CreateAgentInput input);
        Task<List<ProductDiscountDto>> GetProductDiscounts(ProductDiscountInput input);

        Task<PagedResultDto<AgentInfoDto>> GetNetworkAgent(GetAgentNetworkInput input);
        //Task<ProductDiscountDto> GetDiscount(GetProductDiscountDto input);
        Task CreateUserStaff(CreateStaffUserInput input);
        Task UpdateUserStaff(UpdateStaffUserInput input);

        Task<PagedResultDto<UserStaffDto>> GetListUserStaff(GetUserStaffInput input);
        Task<UserStaffDto> GetUserStaffInfo(GetUserStaffInfoInput input);
        Task<ProductDiscountDto> GetProductDiscount(GetProductDiscountDto input);
        Task LockUnLockAccount(LockUnlockRequest input);
        Task<int> GetTotalStaffUser(GetTotalUserStaffInput input);
        Task<ProductFeeDto> GetProductFee(GetFreeAccountInput input);
        Task<LimitProductDetailDto> GetLimitProductPerDay(GetLimitProductPerDayInput input);
    }
}
