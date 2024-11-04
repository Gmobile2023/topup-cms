using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Dtos.Provider;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.Notifications;
using HLS.Topup.Providers;
using HLS.Topup.RequestDtos;
using HLS.Topup.StockManagement.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.StockManagement
{
    public class StockAirtimeManager : TopupDomainServiceBase, IStockAirtimeManager
    {
        private readonly TokenHepper _tokenHepper;

        private readonly TopupAppSession _session;

        //private static bool _inProcess;
        private readonly string _serviceApi;

        private readonly IRepository<Provider> _providerRepository;

        //private readonly Logger _logger = LogManager.GetLogger("IStockAirtimeManager");
        private readonly ILogger<StockAirtimeManager> _logger;
        private readonly INotificationSender _appNotifier;
        private readonly ICacheManager _cacheManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public StockAirtimeManager(IWebHostEnvironment env, TopupAppSession session, TokenHepper commonManager,
            ILogger<StockAirtimeManager> logger, INotificationSender appNotifier,
            IRepository<Provider> providerRepository, ICacheManager cacheManager, IUnitOfWorkManager unitOfWorkManager)
        {
            _tokenHepper = commonManager;
            _logger = logger;
            _appNotifier = appNotifier;
            _providerRepository = providerRepository;
            _cacheManager = cacheManager;
            _unitOfWorkManager = unitOfWorkManager;
            _session = session;
            var appConfiguration = env.GetAppConfiguration();
            _serviceApi = appConfiguration["TopupService:ServiceApi"];
        }

        // list
        public async Task<ApiResponseDto<List<StocksAirtimeDto>>> GetAllStockAirtime(GetAllStockAirtimeRequest input)
        {
            _logger.LogError($"GetAllStockAirtimeRequest request:{input.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                return await client.GetAsync<ApiResponseDto<List<StocksAirtimeDto>>>(input);
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"GetAllStockAirtimeRequest error:{ex}");
                return null;
            }
        }

        // view
        public async Task<ApiResponseDto<StocksAirtimeDto>> GetStockAirtime(GetStockAirtimeRequest input)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                return await client.GetAsync<ApiResponseDto<StocksAirtimeDto>>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        // create
        public async Task<ResponseMessages> CreateStockAirtime(CreateStockAirtimeRequest input)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                return await client.PostAsync<ResponseMessages>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        // update
        public async Task<ResponseMessages> UpdateStockAirtime(UpdateStockAirtimeRequest input)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                return await client.PutAsync<ResponseMessages>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }


        // update
        public async Task<ReponseMessageResultBase<string>> GetAvailableStockAirtime(
            GetAvailableStockAirtimeRequest input)
        {
            try
            {
                _logger.LogInformation($"GetAvailableStockAirtime:{input.ProviderCode}");
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                return await client.GetAsync<ReponseMessageResultBase<string>>(input);

                //return new ReponseMessageResultBase<string>
                //{
                //    ResponseStatus = new StatusResponse(ResponseCodeConst.Success),
                //    Results = "7116.0",
                //};
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"GetAvailableStockAirtime:{input.ProviderCode}");
                return new ReponseMessageResultBase<string>
                {
                    ResponseStatus = new StatusResponse(ResponseCodeConst.Error, "Error")
                };
            }
        }

        public async Task<ReponseMessageResultBase<string>> DepositStockAirtime(ViettelDepositRequest input)
        {
            try
            {
                _logger.LogInformation($"DepositStockAirtime request:{input.ToJson()}");
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(10)
                };
                var rs = await client.PostAsync<ReponseMessageResultBase<string>>(input);
                _logger.LogInformation($"DepositStockAirtime return:{rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation(
                    $"DepositStockAirtime Exception: {ex.Message}|{ex.StackTrace}|{ex.InnerException}");
                return null;
            }
        }


        // list
        public async Task<ApiResponseDto<List<BatchAirtimeDto>>> GetAllBatchAirtime(GetAllBatchAirtimeRequest input)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };

                return await client.GetAsync<ApiResponseDto<List<BatchAirtimeDto>>>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }


        // view
        public async Task<ApiResponseDto<BatchAirtimeDto>> GetBatchAirtime(GetBatchAirtimeRequest input)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                return await client.GetAsync<ApiResponseDto<BatchAirtimeDto>>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        // create
        public async Task<ResponseMessages> CreateBatchAirtime(CreateBatchAirtimeRequest input)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                input.CreatedAccount = _session.AccountCode;
                return await client.PostAsync<ResponseMessages>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        // update
        public async Task<ResponseMessages> UpdateBatchAirtime(UpdateBatchAirtimeRequest input)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                input.ModifiedAccount = _session.AccountCode;
                return await client.PutAsync<ResponseMessages>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public async Task<ResponseMessages> DateteBatchAirtime(DeleteBatchAirtimeRequest input)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                return await client.DeleteAsync<ResponseMessages>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public async Task AutoCheckBalanceProvider()
        {
            try
            {
                using var uow = _unitOfWorkManager.Begin();
                using (_unitOfWorkManager.Current.SetTenantId(null))
                {
                    _logger.LogInformation("AutoCheckBalanceProvider");
                    var lst = await _providerRepository.GetAll().Where(x =>
                            x.ProviderStatus == CommonConst.ProviderStatus.Active && x.MinBalance > 0 &&
                            x.Code != "FAKE")
                        .ToListAsync();
                    if (lst.Any())
                    {
                        foreach (var item in lst)
                        {
                            _logger.LogInformation($"AutoCheckBalanceProvider:{item.Code}-{item.Name}");
                            var response = await GetAvailableStockAirtime(new GetAvailableStockAirtimeRequest
                            {
                                ProviderCode = item.Code
                            });
                            _logger.LogInformation($"AutoCheckBalanceProvider return:{response.ToJson()}");
                            if (response.ResponseStatus.ErrorCode == ResponseCodeConst.Success)
                            {
                                //var balance1 = decimal.Parse(response.Results.ToString(CultureInfo.InvariantCulture)
                                //    ?.Replace(".", ","));

                                var balance = Convert.ToDecimal(double.Parse(response.Results));
                                if (balance <= item.MinBalance)
                                {
                                    _logger.LogWarning(
                                        $"AutoCheckBalanceProvider balance warning {item.Code}-{balance}-{item.MinBalance}");
                                    var message = L("Bot_Send_MinStockAirtime", item.Code,
                                        balance.ToFormat("đ"),
                                        DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                                    await _appNotifier.PublishTeleMessage(new SendTeleMessageRequest
                                    {
                                        Message = message,
                                        Module = "WEB",
                                        Title = "Cảnh báo tồn kho NCC",
                                        BotType = (byte)CommonConst.BotType.Provider,
                                        MessageType = (byte)CommonConst.BotMessageType.Wraning
                                    });
                                }

                                if (balance > 0 && balance < item.MinBalanceToDeposit && item.IsAutoDeposit &&
                                    item.DepositAmount > 0)
                                {
                                    _logger.LogWarning(
                                        $"AutoCheckBalanceProvider auto deposit {item.Code}-{item.DepositAmount}");
                                    //Gửi lệnh nạp tiền
                                    var request = new ViettelDepositRequest()
                                        { Amount = item.DepositAmount, ProviderCode = item.Code };
                                    var depositResponse = await DepositStockAirtime(request);
                                    _logger.LogInformation(
                                        $"AutoCheckBalanceProvider deposit return :  {item.Code}-{response.ToJson()}");
                                    await _appNotifier.PublishTeleMessage(new SendTeleMessageRequest
                                    {
                                        Message = depositResponse.ResponseStatus.ErrorCode == ResponseCodeConst.Success
                                            ? $"Tự động nạp {item.DepositAmount.ToFormat("đ")} tiền vào NCC: {item.Code} thành công"
                                            : $"Không thể nạp tiền tự động vào cho NCC: {item.Code}. Vui lòng kiểm tra hoặc thử nạp bằng tay.\nThông tin lỗi:{depositResponse.ResponseStatus.ToJson()}\nLỗi NCC:{depositResponse.Results}",
                                        Module = "WEB",
                                        Title = $"Thông báo nạp tiền tự động NCC {item.Code}",
                                        BotType = (byte)CommonConst.BotType.Provider,
                                        MessageType = depositResponse.ResponseStatus.ErrorCode ==
                                                      ResponseCodeConst.Success
                                            ? (byte)CommonConst.BotMessageType.Message
                                            : (byte)CommonConst.BotMessageType.Error
                                    });
                                }
                            }
                            else
                            {
                                _logger.LogError(
                                    $"AutoCheckBalanceProvider not success return :  {item.Code}-{response.ToJson()}");
                                await _appNotifier.PublishTeleMessage(new SendTeleMessageRequest
                                {
                                    Message =
                                        $"Kiểm tra số dư NCC :{item.Code} không thành công\nMessage:{response.ResponseStatus.ToJson()}",
                                    Module = "WEB",
                                    Title = "Cảnh báo tồn kho NCC",
                                    BotType = (byte)CommonConst.BotType.Provider,
                                    MessageType = (byte)CommonConst.BotMessageType.Error
                                });
                            }
                        }
                    }
                }

                await uow.CompleteAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Auto check balance error:{e}");
            }
        }

        public async Task AutoCheckBalanceProvider_bak()
        {
            _logger.LogInformation("AutoCheckBalanceProvider");
            var lst = await GetAirProviderCache();
            if (lst != null)
            {
                foreach (var item in lst.Where(item => item.ProviderCode != "FAKE"))
                {
                    _logger.LogInformation($"AutoCheckBalanceProvider:{item.ProviderCode}-{item.ProviderName}");
                    var response = await GetAvailableStockAirtime(new GetAvailableStockAirtimeRequest
                    {
                        ProviderCode = item.ProviderCode
                    });
                    decimal balance = 0;
                    _logger.LogInformation($"AutoCheckBalanceProvider return:{response.ToJson()}");
                    if (response.ResponseStatus.ErrorCode == ResponseCodeConst.Success)
                    {
                        balance = decimal.Parse(response.Results.ToString(CultureInfo.InvariantCulture)
                            ?.Replace(".", ","));
                    }

                    if (item.MinLimitAirtime <= 0 || balance >= item.MinLimitAirtime) continue;
                    var message = L("Bot_Send_MinStockAirtime", item.ProviderCode,
                        balance.ToFormat("đ"),
                        DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _logger.LogInformation($"BalanceAirtime:{balance}");
                    await _appNotifier.PublishTeleMessage(new SendTeleMessageRequest
                    {
                        Message = message,
                        Module = "WEB",
                        Title = "Cảnh báo tồn kho NCC",
                        BotType = (byte)CommonConst.BotType.Provider,
                        MessageType = (byte)CommonConst.BotMessageType.Wraning
                    });
                }
            }
        }

        private async Task<List<StocksAirtimeDto>> GetAirProviderCache()
        {
            try
            {
                var rs = await GetAllStockAirtime(new GetAllStockAirtimeRequest
                {
                    Limit = int.MaxValue,
                    Offset = 0,
                    Status = 99
                });
                if (rs?.ResponseCode != "01")
                    return null;
                var data = rs.Payload;
                if (data.Any())
                {
                    var providersCode = data.Select(x => x.KeyCode);
                    var providers = await _providerRepository.GetAll()
                        .Where(x => x.ProviderStatus == CommonConst.ProviderStatus.Active)
                        .Where(x => providersCode.Contains(x.Code))
                        .ToListAsync();
                    foreach (var item in data)
                    {
                        var p = providers.FirstOrDefault(x => x.Code == item.KeyCode);
                        item.ProviderCode = p != null ? p.Code : item.KeyCode;
                        item.ProviderName = p != null ? p.Name : item.KeyCode;
                    }

                    return data;
                }

                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}