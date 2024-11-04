using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using HLS.Topup.Authorization.Accounts.Dto;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Authorization.Users.Dto;
using HLS.Topup.Common.Dto;
using HLS.Topup.Dtos.Common;
using HLS.Topup.Products.Dtos;
using HLS.Topup.Providers.Dtos;
using HLS.Topup.Services.Dtos;

namespace HLS.Topup.Common
{
    public interface IPrivateAppService : IApplicationService
    {
        Task<UserInfoDto> GetUserInfoQuery(GetUserInfoRequest input);

        Task<UserInfoDto> GetUserFullInfoQuery(GetUserInfoRequest input);

        Task<UserInfoDto> GetUserInfoQueryByAccountCode(string accountCode);

        Task<object> GetUserPeriodInfoQuery(GetUserPeriodRequest input);        

        Task<ProductInfoDto> GetProductInfo(ProductInfoInput input);
        Task<ServiceDto> GetService(ServiceInfoInput input);
        Task<ProviderDto> GetProvider(ProviderInfoInput input);
        Task<VendorTransDto> GetVendorTrans(VendorTransInfoInput input);

        Task<UserLimitDebtDto> GetLimitDebtAccount(GetLimitRequest request);

        Task<UserInfoSaleDto> GetSaleAssignInfo(int userId);
        Task<ResponseMessages> HandlerDepositSmsReceiver(SmsReceiverDto input);

        Task<List<AgentTypeDto>> GetAgenTypeInfo();
    }
}
