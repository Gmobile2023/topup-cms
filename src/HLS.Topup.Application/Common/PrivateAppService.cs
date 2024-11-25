using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Authorization.Users.Dto;
using HLS.Topup.Banks;
using HLS.Topup.Categories;
using Microsoft.EntityFrameworkCore;
using HLS.Topup.Common.Dto;
using HLS.Topup.Configuration;
using HLS.Topup.DiscountManager;
using HLS.Topup.Editions;
using HLS.Topup.Products;
using HLS.Topup.Products.Dtos;
using HLS.Topup.Services.Dtos;
using HLS.Topup.Address;
using HLS.Topup.Dtos.Common;
using HLS.Topup.Providers;
using HLS.Topup.Providers.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ServiceStack;
using HLS.Topup.Sale;
using System;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Abp.UI;
using HLS.Topup.Deposits;
using HLS.Topup.Notifications;
using HLS.Topup.RequestDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HLS.Topup.Authorization.Accounts.Dto;
using HLS.Topup.StockManagement;

namespace HLS.Topup.Common
{
    /// <summary>
    /// Hàm này k authen. Để cho core gọi priviate
    /// </summary>
    public class PrivateAppService : TopupAppServiceBase, IPrivateAppService
    {
        private readonly IRepository<Services.Service> _serviceRepository;
        private readonly IRepository<Providers.Provider> _providerRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<District> _districtRepository;
        private readonly IRepository<Ward> _wardRepository;
        private readonly IRepository<UserProfile> _profileRepository;
        private readonly IRepository<Bank> _bankRepository;
        private readonly IRepository<Deposit> _depositRepository;
        private readonly ICommonManger _commonManger;
        private readonly IRepository<SaleLimitDebt> _saleLimitDebtRepository;
        private readonly ISaleManManager _saleManManager;
        private readonly ILogger<PrivateAppService> _logger;
        private readonly IDepositManager _depositManager;
        private readonly INotificationSender _appNotifier;
       // private readonly IStockAirtimeManager _stockAirtimeManager;

        public PrivateAppService(IRepository<Product> productRepository,
            IRepository<City> cityRepository, IRepository<District> districtRepository,
            IRepository<Ward> wardRepository, ICommonManger commonManger,
            IRepository<SaleLimitDebt> saleLimitDebtRepository,
            IRepository<Services.Service> serviceRepository, IRepository<Provider> providerRepository,
            IRepository<UserProfile> profileRepository,
            IRepository<Bank> bankRepository,
            IRepository<Deposit> depositRepository,
            IDepositManager depositManager,
            INotificationSender appNotifier,
            ISaleManManager saleManManager, ILogger<PrivateAppService> logger
           // IStockAirtimeManager stockAirtimeManager
            )
        {
            _productRepository = productRepository;
            _commonManger = commonManger;
            _serviceRepository = serviceRepository;
            _providerRepository = providerRepository;
            _cityRepository = cityRepository;
            _districtRepository = districtRepository;
            _wardRepository = wardRepository;
            _saleLimitDebtRepository = saleLimitDebtRepository;
            _saleManManager = saleManManager;
            _logger = logger;
            _profileRepository = profileRepository;
            _bankRepository = bankRepository;
            _depositRepository = depositRepository;
            _depositManager = depositManager;
            _appNotifier = appNotifier;
           // _stockAirtimeManager = stockAirtimeManager;
        }


        public async Task<UserInfoDto> GetUserInfoQuery(GetUserInfoRequest input)
        {
            _logger.LogInformation($"GetUserInfoQuery:{input.ToJson()}");
            User user = null;
            if (!string.IsNullOrEmpty(input.AccountCode))
                user = await UserManager.GetUserByAccountCodeAsync(input.AccountCode);
            if (!string.IsNullOrEmpty(input.UserName))
                user = await UserManager.GetUserByUserNameAsync(input.UserName);
            if (!string.IsNullOrEmpty(input.Email))
                user = await UserManager.GetUserByEmailAsync(input.Email);
            if (!string.IsNullOrEmpty(input.PhoneNumber))
                user = await UserManager.GetUserByMobileAsync(input.PhoneNumber);
            if (input.UserId != null && input.UserId != 0)
                user = await UserManager.GetUserByIdAsync(input.UserId ?? 0);
            if (!string.IsNullOrEmpty(input.Search))
                user = await UserManager.GetUserAnyFieldAsync(input.Search);
            if (user == null)
                return null;
            //// if (user.IsAccountSystem())
            ////     return null;

            //UserUnit unit = null;
            //if (user != null)
            //{
            //    unit = (from p in _profileRepository.GetAll().Where(c => c.UserId == user.Id)
            //            join c in _cityRepository.GetAll() on p.CityId equals c.Id into gc
            //            from city in gc.DefaultIfEmpty()
            //            join d in _districtRepository.GetAll() on p.DistrictId equals d.Id into dg
            //            from district in dg.DefaultIfEmpty()
            //            join w in _wardRepository.GetAll() on p.WardId equals w.Id into wg
            //            from ward in wg.DefaultIfEmpty()
            //            where p.UserId == user.Id
            //            select new UserUnit
            //            {
            //                CityId = p.CityId ?? 0,
            //                CityName = city != null ? city.CityName : "",
            //                CityCode = city != null ? city.CityCode : "",
            //                DistrictId = p.DistrictId ?? 0,
            //                DistrictName = district != null ? district.DistrictName : "",
            //                DistrictCode = district != null ? district.DistrictCode : "",
            //                WardId = p.WardId ?? 0,
            //                WardName = ward != null ? ward.WardName : "",
            //                WardCode = ward != null ? ward.WardCode : "",
            //                IdIdentity = p.IdIdentity,
            //            }).FirstOrDefault();


            //}

            var info = user.ConvertTo<UserInfoDto>();
            // info.Unit = unit;
            return info;
        }

        public async Task<UserInfoDto> GetUserFullInfoQuery(GetUserInfoRequest input)
        {
            _logger.LogInformation($"GetUserFullInfoQuery:{input.ToJson()}");
            User user = null;
            if (!string.IsNullOrEmpty(input.AccountCode))
                user = await UserManager.GetUserByAccountCodeAsync(input.AccountCode);
            if (!string.IsNullOrEmpty(input.UserName))
                user = await UserManager.GetUserByUserNameAsync(input.UserName);
            if (!string.IsNullOrEmpty(input.Email))
                user = await UserManager.GetUserByEmailAsync(input.Email);
            if (!string.IsNullOrEmpty(input.PhoneNumber))
                user = await UserManager.GetUserByMobileAsync(input.PhoneNumber);
            if (input.UserId != null && input.UserId != 0)
                user = await UserManager.GetUserByIdAsync(input.UserId ?? 0);
            if (!string.IsNullOrEmpty(input.Search))
                user = await UserManager.GetUserAnyFieldAsync(input.Search);
            if (user == null)
                return null;
            UserUnit unit = null;
            if (user != null)
            {
                unit = (from p in _profileRepository.GetAll().Where(c => c.UserId == user.Id)
                        join c in _cityRepository.GetAll() on p.CityId equals c.Id into gc
                        from city in gc.DefaultIfEmpty()
                        join d in _districtRepository.GetAll() on p.DistrictId equals d.Id into dg
                        from district in dg.DefaultIfEmpty()
                        join w in _wardRepository.GetAll() on p.WardId equals w.Id into wg
                        from ward in wg.DefaultIfEmpty()
                        where p.UserId == user.Id
                        select new UserUnit
                        {
                            CityId = p.CityId ?? 0,
                            CityName = city != null ? city.CityName : "",
                            CityCode = city != null ? city.CityCode : "",
                            DistrictId = p.DistrictId ?? 0,
                            DistrictName = district != null ? district.DistrictName : "",
                            DistrictCode = district != null ? district.DistrictCode : "",
                            WardId = p.WardId ?? 0,
                            WardName = ward != null ? ward.WardName : "",
                            WardCode = ward != null ? ward.WardCode : "",
                            IdIdentity = p.IdIdentity,
                            ChatId = p.ChatId
                        }).FirstOrDefault();
            }

            var info = user.ConvertTo<UserInfoDto>();
            info.Unit = unit;
            if (user.AccountType == CommonConst.SystemAccountType.Agent
                || user.AccountType == CommonConst.SystemAccountType.MasterAgent
                || user.AccountType == CommonConst.SystemAccountType.Company)
            {
                var assignInfo = await _saleManManager.GetSaleAssignInfo(Convert.ToInt32(info.Id));
                if (assignInfo != null)
                {
                    info.SaleCode = assignInfo.SaleCode;
                    info.LeaderCode = assignInfo.SaleLeaderCode;
                }
            }

            return info;
        }

        public async Task<object> GetUserPeriodInfoQuery(GetUserPeriodRequest input)
        {
            _logger.LogInformation($"GetUserPeriodInfoQuery:{input.ToJson()}");
            if (string.IsNullOrEmpty(input.AgentCode))
            {
                var list = _profileRepository.GetAll().Include(e => e.UserFk)
                    .Where(c => c.UserFk != null &&
                                (c.UserFk.AccountType == CommonConst.SystemAccountType.MasterAgent
                                 || c.UserFk.AccountType == CommonConst.SystemAccountType.Agent))
                    .WhereIf(!string.IsNullOrWhiteSpace(input.AgentCode),
                        e => e.UserFk != null && e.UserFk.AccountCode == input.AgentCode)
                    .WhereIf(input.AgentType != CommonConst.AgentType.Default,
                        e => e.UserFk != null && e.UserFk.AgentType == input.AgentType);

                var lit = (from x in list.ToList()
                           select new UserInfoPeriodDto()
                           {
                               AgentCode = x.UserFk.AccountCode,
                               AgentType = x.UserFk.AgentType,
                               EmailAddress = x.UserFk.EmailAddress,
                               FullName = x.UserFk.FullName,
                               Period = x.PeriodCheck,
                               PhoneNumber = x.UserFk.PhoneNumber,
                               UserName = x.UserFk.UserName,
                               ContractNumber = x.ContractNumber,
                               EmailReceives = x.EmailReceives,
                               FolderFtp = x.FolderFtp,
                               SigDate = x.SigDate,
                           }).ToList();

                return lit.ToJson();
            }
            else
            {
                List<UserInfoPeriodDto> lit = new List<UserInfoPeriodDto>();
                var user = await UserManager.GetUserByAccountCodeAsync(input.AgentCode);
                var profile = await _profileRepository.FirstOrDefaultAsync(x => x.UserId == user.Id);
                lit.Add(new UserInfoPeriodDto()
                {
                    AgentCode = user.AccountCode,
                    EmailAddress = user.EmailAddress,
                    AgentType = user.AgentType,
                    FullName = user.FullName,
                    Period = profile != null ? profile.PeriodCheck : 0,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    ContractNumber = profile?.ContractNumber,
                    EmailReceives = profile?.EmailReceives,
                    FolderFtp = profile?.FolderFtp,
                    SigDate = profile?.SigDate,
                });

                return lit.ToJson();
            }
        }

        public async Task<UserInfoDto> GetUserInfoQueryByAccountCode(string accountCode)
        {
            _logger.LogInformation($"GetUserInfoQueryByAccountCode:{accountCode}");
            var user = await UserManager.GetUserByAccountCodeAsync(accountCode);
            return user.ConvertTo<UserInfoDto>();
        }

        public async Task<ProductInfoDto> GetProductInfo(ProductInfoInput input)
        {
            _logger.LogInformation($"GetProductInfo:{input.ToJson()}");
            var pro = await _productRepository.GetAllIncluding(x => x.CategoryFk)
                .FirstOrDefaultAsync(x => x.ProductCode == input.ProductCode);
            var item = pro?.ConvertTo<ProductInfoDto>();
            if (item != null)
            {
                item.CategoryName = pro.CategoryFk.CategoryName;
                item.CategoryCode = pro.CategoryFk.CategoryCode;
                item.ServiceId = pro.CategoryFk.ServiceId ?? 0;
                if (item.ServiceId > 0)
                {
                    var service = _serviceRepository.Get(item.ServiceId);
                    if (service != null)
                    {
                        item.ServiceCode = service.ServiceCode;
                        item.ServiceName = service.ServicesName;
                    }
                }
            }

            return item;
        }

        public async Task<ServiceDto> GetService(ServiceInfoInput input)
        {
            _logger.LogInformation($"GetService:{input.ToJson()}");
            var item = await _serviceRepository.FirstOrDefaultAsync(x => x.ServiceCode == input.ServiceCode);
            return item?.ConvertTo<ServiceDto>();
        }

        public async Task<ProviderDto> GetProvider(ProviderInfoInput input)
        {
            _logger.LogInformation($"GetProvider:{input.ToJson()}");
            var item = await _providerRepository.FirstOrDefaultAsync(x => x.Code == input.ProviderCode);
            return item?.ConvertTo<ProviderDto>();
        }

        public async Task<VendorTransDto> GetVendorTrans(VendorTransInfoInput input)
        {
            _logger.LogInformation($"GetVendorTrans:{input.ToJson()}");
            var items = await _commonManger.GetListVendorTrans();
            if (items != null && items.Count > 0)
                return items.FirstOrDefault(x => x.Code == input.Code);
            return null;
        }

        public async Task<UserLimitDebtDto> GetLimitDebtAccount(GetLimitRequest request)
        {
            try
            {
                _logger.LogInformation($"GetLimitRequest:{request.ToJson()}");
                var user = await UserManager.GetUserByAccountCodeAsync(request.AccountCode);
                if (user == null)
                    return null;
                var limit = await _saleLimitDebtRepository.GetAll().Where(c =>
                        c.UserId == user.Id
                        && c.CreationTime <= DateTime.Now
                        && c.Status == CommonConst.DebtLimitAmountStatus.Active)
                    .OrderByDescending(c => c.CreationTime).FirstOrDefaultAsync();
                var rs = new UserLimitDebtDto()
                {
                    Limit = limit.LimitAmount,
                    DebtAge = limit.DebtAge,
                };
                _logger.LogInformation($"GetLimitRequest return:{rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserInfoSaleDto> GetSaleAssignInfo(int userId)
        {
            _logger.LogInformation($"GetSaleAssignInfo Request:{userId}");
            var user = await _saleManManager.GetSaleAssignInfo(userId);
            _logger.LogInformation($"GetSaleAssignInfo Return:{user?.UserSaleId}");
            return user.ConvertTo<UserInfoSaleDto>();
        }

        //public async Task<string> GetAirtimeTest(GetTestRequest rquest)
        //{
        //   await _stockAirtimeManager.AutoCheckBalanceProvider();
        //    return string.Empty;
        //}
        public async Task<List<AgentTypeDto>> GetAgenTypeInfo()
        {

            var arrays = new List<AgentTypeDto>();
            arrays.Add(new AgentTypeDto()
            {
                AgentTypeId = 1,
                AgentTypeName = "Đại lý"
            });
            arrays.Add(new AgentTypeDto()
            {
                AgentTypeId = 2,
                AgentTypeName = "Đại lý API"
            });
            arrays.Add(new AgentTypeDto()
            {
                AgentTypeId = 3,
                AgentTypeName = "Đại lý công ty"
            });
            arrays.Add(new AgentTypeDto()
            {
                AgentTypeId = 4,
                AgentTypeName = "Đại lý Tổng"
            });
            arrays.Add(new AgentTypeDto()
            {
                AgentTypeId = 5,
                AgentTypeName = "Đại lý cấp 1"
            });
            arrays.Add(new AgentTypeDto()
            {
                AgentTypeId = 6,
                AgentTypeName = "Đại lý sỉ"
            });
            return arrays;
        }

        private async Task<ResponseMessages> ResponseLog(string code, string message, SmsReceiverDto input,
            SmsInformation obj = null)
        {
            var notify = "";
            if (obj != null)
                notify = "Ngân hàng: " + (obj.bank) +
                         (string.IsNullOrEmpty(obj.requestCode) ? "" : $"\nMã nạp: {obj.requestCode}") +
                         "\nSố tiền nạp: " + (obj.amount.ReplaceAll("(", "").ReplaceAll(")", "")) +
                         "\nNội dung: " + (obj.note ?? "") +
                         (code == "0" ? $"\nLỗi: {(message ?? "")}" : $"\nMã đại lý: {(obj.account ?? "")}");

            if (obj != null)
                await _appNotifier.PublishTeleMessage(new SendTeleMessageRequest
                {
                    Message = notify,
                    Module = "API",
                    Title = code == "1" ? "Duyệt yêu cầu nạp tiền ngân hàng thành công" : "Cảnh báo Duyệt yêu cầu nạp tiền ngân hàng không thành công",
                    BotType = (byte)CommonConst.BotType.Deposit,
                    MessageType = code == "1" ? (byte)CommonConst.BotMessageType.Message : (byte)CommonConst.BotMessageType.Wraning,
                    Code = code
                });

            _logger.LogInformation($"HandlerDepositSmsReceiver Response:{(new { code, message, input, obj, notify }).ToJson()}");
            if (code != ResponseCodeConst.Success)
                throw new UserFriendlyException(message);
            else
                return new ResponseMessages()
                {
                    ResponseCode = code,
                    ResponseMessage = message,
                    Payload = obj
                };
        }

        [HttpGet]
        public async Task<ResponseMessages> HandlerDepositSmsReceiver(SmsReceiverDto input)
        {
            _logger.LogInformation($"HandlerDepositSmsReceiver Request:{input.ToJson()}");
            if (string.IsNullOrEmpty(input.From))
                return await ResponseLog(ResponseCodeConst.Error, "SĐT gửi không phù hợp", input);
            if (string.IsNullOrEmpty(input.To))
                return await ResponseLog(ResponseCodeConst.Error, "SĐT nhận không phù hợp", input);
            var bank = await _bankRepository.GetAll()
                .Where(s => s.SmsGatewayNumber == input.From && s.SmsPhoneNumber == input.To)
                .OrderByDescending(c => c.CreationTime).FirstOrDefaultAsync();
            if (bank == null)
                return await ResponseLog(ResponseCodeConst.Error, "SMS không phù hợp hoặc ngân hàng chưa được cấu hình",
                    input);
            var smsRegex = new Regex(bank.SmsSyntax);
            var smsMatch = smsRegex.Match(input.Message);
            // obj
            var obj = new SmsInformation()
            {
                bank = bank.ShortName,
                stk = smsMatch.Groups["stk"].Value.Trim(),
                amount = smsMatch.Groups["amount"].Value.Trim(),
                date = smsMatch.Groups["date"].Value.Trim(),
                balance = smsMatch.Groups["balance"].Value.Trim(),
                note = smsMatch.Groups["note"].Value.Trim(),
                requestCode = "",
            };

            var amountStr = smsMatch.Groups["amount"].Value.Trim().ReplaceAll(",", "");
            if (string.IsNullOrEmpty(amountStr))
                return await ResponseLog(ResponseCodeConst.Error, "Nội dung SMS không phù hợp", input);
            if (amountStr.Contains(")"))
                amountStr = amountStr.ReplaceAll("(", "").ReplaceAll(")", "");
            if (amountStr.Contains("-"))
                return await ResponseLog(ResponseCodeConst.Error, "SMS của GD trừ tiền tài khoản", input);
            // ghi chú
            var note = obj.note;
            if (string.IsNullOrEmpty(note))
                return await ResponseLog(ResponseCodeConst.Error, "Không có dữ liệu ghi chú chuyển tiền", input, obj);
            if (note.ToString().Length < 8)
                return await ResponseLog(ResponseCodeConst.Error, "Mã nạp tiền không chính xác", input, obj);
            var requestCode = note.Substring(0, 8);
            obj.requestCode = requestCode;
            if (string.IsNullOrEmpty(requestCode) && !requestCode.StartsWith("NP"))
                return await ResponseLog(ResponseCodeConst.Error, "Không có thông tin mã nạp tiền", input, obj);

            // depositAll
            var depositAll = await _depositRepository.GetAll()
                .Include(x => x.UserFk)
                .Where(s => s.RequestCode == requestCode)
                .OrderByDescending(c => c.CreationTime)
                .ToListAsync();

            if (!depositAll.Any())
                return await ResponseLog(ResponseCodeConst.Error, $"Không tìm thấy yêu cầu nạp tiền",
                    input, obj);
            if (depositAll.Count > 1)
                return await ResponseLog(ResponseCodeConst.Error, $"Mã giao dịch trùng, không xác định được yêu cầu nạp tiền: {requestCode}", input, obj);

            // deposit
            var deposit = depositAll.FirstOrDefault();
            if (deposit == null)
                return await ResponseLog(ResponseCodeConst.Error, $"Không tìm thấy yêu cầu nạp tiền: {requestCode}", input, obj);
            obj.account = deposit.UserFk.AccountCode;
            if (deposit.Status != CommonConst.DepositStatus.Pending)
                return await ResponseLog(ResponseCodeConst.Error, $"Trạng thái yêu cầu nạp tiền không phù hợp", input, obj);

            if (deposit.BankId != bank.Id)
                return await ResponseLog(ResponseCodeConst.Error, $"Ngân hàng của yêu cầu nạp tiền không phù hợp",
                    input, obj);
            var amount = decimal.Parse(amountStr.Replace("+", ""));
            if (deposit.Amount != amount)
                return await ResponseLog(ResponseCodeConst.Error, $"Số tiền nạp từ SMS không phù hợp với yêu cầu nạp tiền", input, obj);
            try
            {
                await _depositManager.ApprovalDeposit(deposit.TransCode, deposit.TransCode, deposit.Description, null);
            }
            catch (Exception e)
            {
                return await ResponseLog(ResponseCodeConst.Error, $"Có lỗi trong quá trình duyệt tiền", input, obj);
            }

            return await ResponseLog(ResponseCodeConst.Success, "Thành công", input, obj);
        }

        class SmsInformation
        {
            public string bank { get; set; }
            public string stk { get; set; }
            public string amount { get; set; }
            public string date { get; set; }
            public string balance { get; set; }
            public string note { get; set; }
            public string requestCode { get; set; }
            public string account { get; set; }
        }
    }
}
