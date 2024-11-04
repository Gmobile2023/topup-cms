using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.Dtos.Configuration;
using HLS.Topup.Providers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Abp.Linq.Extensions;
using HLS.Topup.Common;
using System;
using HLS.Topup.Services;

namespace HLS.Topup.Configuration
{
    public class ServiceConfigurationManager : TopupDomainServiceBase, IServiceConfigurationManager
    {
        private readonly IRepository<ServiceConfiguration> _serviceConfigRepository;
        private readonly IRepository<Provider> _providerRepository;

        public ServiceConfigurationManager(IRepository<Provider> providerRepository,
            IRepository<ServiceConfiguration> serviceConfigRepository)
        {
            _providerRepository = providerRepository;
            _serviceConfigRepository = serviceConfigRepository;
        }

        public async Task<List<ServiceConfiguationDto>> GetServiceConfiguations(string accountCode, string serviceCode,
            string categoryCode, bool isApplySlowTrans = false, bool masterAccount = false)
        {
            try
            {
                if (!string.IsNullOrEmpty(accountCode))
                {
                    var query = _serviceConfigRepository.GetAllIncluding(x => x.ServiceFk)
                        .Include(x => x.CategoryFk)
                        .Include(x => x.UserFk)
                        .Include(x => x.ProviderFk)
                        .Where(x => x.CategoryFk.CategoryCode == categoryCode
                                    && x.ServiceFk.ServiceCode == serviceCode
                                    && x.IsOpened && x.ProviderFk.ProviderStatus == CommonConst.ProviderStatus.Active
                                    && x.ServiceFk.Status == ServiceStatus.Active
                                    && x.CategoryFk.Status == CommonConst.CategoryStatus.Active
                                    && x.UserFk.AccountCode == accountCode
                                    && x.ServiceId != null
                                    && x.CategoryId != null
                                    && x.ProductId == null
                                    && x.UserId != null)
                        .WhereIf(masterAccount, x => x.ProviderFk.ParentProvider == null);

                    var item = await query.Select(x => new ServiceConfiguationDto
                    {
                        Description = x.Description,
                        Priority = x.Priority,
                        Name = x.Name,
                        AccountCode = x.UserFk.AccountCode,
                        CategoryCode = x.CategoryFk.CategoryCode,
                        IsOpened = x.IsOpened,
                        ProviderCode = x.ProviderFk.Code,
                        ProviderName = x.ProviderFk.Name,
                        ServiceCode = x.ServiceFk.ServiceCode,
                        TransCodeConfig = x.ProviderFk.TransCodeConfig,

                        //11062022
                        ProviderMaxWaitingTimeout = x.ProviderMaxWaitingTimeout,
                        ProviderSetTransactionTimeout = x.ProviderSetTransactionTimeout,
                        IsEnableResponseWhenJustReceived = x.IsEnableResponseWhenJustReceived,
                        StatusResponseWhenJustReceived = x.StatusResponseWhenJustReceived,
                        IsLastConfiguration = x.IsLastConfiguration,

                        //IsSlowTrans = (x.ProviderFk.IsSlowTrans ?? false) && isApplySlowTrans
                        IsSlowTrans = x.IsEnableResponseWhenJustReceived ?? false,
                        WaitingTimeResponseWhenJustReceived = x.WaitingTimeResponseWhenJustReceived,
                        IsAutoDeposit = x.ProviderFk.IsAutoDeposit,
                        IsRoundRobinAccount = x.ProviderFk.IsRoundRobinAccount,
                        MinBalanceToDeposit = x.ProviderFk.MinBalanceToDeposit,
                        MinBalance = x.ProviderFk.MinBalance,
                        ParentProvider = x.ProviderFk.ParentProvider,
                        DepositAmount = x.ProviderFk.DepositAmount,
                        RateRunning = x.RateRunning ?? 0,
                        WorkShortCode = x.ProviderFk.WorkShortCode,
                        AllowTopupReceiverType = x.AllowTopupReceiverType
                    }).OrderBy(x => x.Priority).ThenBy(x => x.CategoryCode).ToListAsync();
                    return item.Any() ? item : null;
                }
                else
                {
                    var query = _serviceConfigRepository.GetAllIncluding(x => x.ServiceFk)
                        .Include(x => x.CategoryFk)
                        .Include(x => x.ProviderFk)
                        .Where(x => x.CategoryFk.CategoryCode == categoryCode &&
                                    x.ServiceFk.ServiceCode == serviceCode
                                    && x.IsOpened && x.ProviderFk.ProviderStatus == CommonConst.ProviderStatus.Active
                                    && x.ServiceFk.Status == ServiceStatus.Active
                                    && x.CategoryFk.Status == CommonConst.CategoryStatus.Active
                                    && x.ServiceId != null && x.CategoryId != null
                                    && x.ProductId == null
                                    && x.UserId == null).WhereIf(masterAccount,
                            x => x.ProviderFk.ParentProvider == null);

                    var item = await query.Select(x => new ServiceConfiguationDto
                    {
                        Description = x.Description,
                        Priority = x.Priority,
                        Name = x.Name,
                        CategoryCode = x.CategoryFk.CategoryCode,
                        IsOpened = x.IsOpened,
                        ServiceCode = x.ServiceFk.ServiceCode,
                        ProviderCode = x.ProviderFk.Code,
                        ProviderName = x.ProviderFk.Name,
                        TransCodeConfig = x.ProviderFk.TransCodeConfig,

                        //11062022
                        ProviderMaxWaitingTimeout = x.ProviderMaxWaitingTimeout,
                        ProviderSetTransactionTimeout = x.ProviderSetTransactionTimeout,
                        IsEnableResponseWhenJustReceived = x.IsEnableResponseWhenJustReceived,
                        StatusResponseWhenJustReceived = x.StatusResponseWhenJustReceived,
                        IsLastConfiguration = x.IsLastConfiguration,

                        //IsSlowTrans = (x.ProviderFk.IsSlowTrans ?? false) && isApplySlowTrans
                        IsSlowTrans = x.IsEnableResponseWhenJustReceived ?? false,
                        WaitingTimeResponseWhenJustReceived = x.WaitingTimeResponseWhenJustReceived,
                        IsAutoDeposit = x.ProviderFk.IsAutoDeposit,
                        IsRoundRobinAccount = x.ProviderFk.IsRoundRobinAccount,
                        MinBalanceToDeposit = x.ProviderFk.MinBalanceToDeposit,
                        MinBalance = x.ProviderFk.MinBalance,
                        ParentProvider = x.ProviderFk.ParentProvider,
                        DepositAmount = x.ProviderFk.DepositAmount,
                        AllowTopupReceiverType = x.AllowTopupReceiverType,
                        RateRunning = x.RateRunning ?? 0,
                        WorkShortCode = x.ProviderFk.WorkShortCode,
                    }).OrderBy(x => x.Priority).ThenBy(x => x.CategoryCode).ToListAsync();
                    return item.Any() ? item : null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<ServiceConfiguationDto>> GetServiceConfiguations(string accountCode, string serviceCode,
            string categoryCode,
            string productCode, bool isApplySlowTrans = false, bool masterAccount = false)
        {
            try
            {
                if (!string.IsNullOrEmpty(accountCode))
                {
                    var query = _serviceConfigRepository.GetAllIncluding(x => x.ServiceFk)
                        .Include(x => x.CategoryFk)
                        .Include(x => x.ProviderFk)
                        .Include(x => x.UserFk)
                        .Include(x => x.ProductFk)
                        .Where(x => x.CategoryFk.CategoryCode == categoryCode
                                    && x.ServiceFk.ServiceCode == serviceCode
                                    && x.ProductFk.ProductCode == productCode
                                    && x.UserFk.AccountCode == accountCode
                                    && x.ServiceFk.Status == ServiceStatus.Active
                                    && x.CategoryFk.Status == CommonConst.CategoryStatus.Active
                                    && x.ProductFk.Status == CommonConst.ProductStatus.Active
                                    && x.IsOpened && x.ProviderFk.ProviderStatus == CommonConst.ProviderStatus.Active &&
                                    x.CategoryId != null && x.ServiceId != null && x.UserId != null &&
                                    x.ProductId != null).WhereIf(masterAccount,
                            x => x.ProviderFk.ParentProvider == null);

                    var item = await query.Select(x => new ServiceConfiguationDto
                    {
                        AccountCode = x.UserFk.AccountCode,
                        ProductCode = x.ProductFk.ProductCode,
                        Description = x.Description,
                        Priority = x.Priority,
                        Name = x.Name,
                        CategoryCode = x.CategoryFk.CategoryCode,
                        ProviderCode = x.ProviderFk.Code,
                        ProviderName = x.ProviderFk.Name,
                        IsOpened = x.IsOpened,
                        ServiceCode = x.ServiceFk.ServiceCode,
                        ProductValue = x.ProductFk.ProductValue,
                        TransCodeConfig = x.ProviderFk.TransCodeConfig,

                        //11062022
                        ProviderMaxWaitingTimeout = x.ProviderMaxWaitingTimeout,
                        ProviderSetTransactionTimeout = x.ProviderSetTransactionTimeout,
                        IsEnableResponseWhenJustReceived = x.IsEnableResponseWhenJustReceived,
                        StatusResponseWhenJustReceived = x.StatusResponseWhenJustReceived,
                        IsLastConfiguration = x.IsLastConfiguration,

                        //IsSlowTrans = (x.ProviderFk.IsSlowTrans ?? false) && isApplySlowTrans
                        IsSlowTrans = x.IsEnableResponseWhenJustReceived ?? false,
                        WaitingTimeResponseWhenJustReceived = x.WaitingTimeResponseWhenJustReceived,
                        IsAutoDeposit = x.ProviderFk.IsAutoDeposit,
                        IsRoundRobinAccount = x.ProviderFk.IsRoundRobinAccount,
                        MinBalanceToDeposit = x.ProviderFk.MinBalanceToDeposit,
                        MinBalance = x.ProviderFk.MinBalance,
                        ParentProvider = x.ProviderFk.ParentProvider,
                        DepositAmount = x.ProviderFk.DepositAmount,
                        AllowTopupReceiverType = x.AllowTopupReceiverType,
                        RateRunning = x.RateRunning ?? 0,
                        WorkShortCode = x.ProviderFk.WorkShortCode,
                    }).OrderBy(x => x.Priority).ThenBy(x => x.CategoryCode).ToListAsync();
                    return item.Any() ? item : null;
                }
                else
                {
                    var query = _serviceConfigRepository.GetAllIncluding(x => x.ServiceFk)
                        .Include(x => x.CategoryFk)
                        .Include(x => x.ProviderFk)
                        .Include(x => x.ProductFk)
                        .Where(x => x.CategoryFk.CategoryCode == categoryCode
                                    && x.ServiceFk.ServiceCode == serviceCode
                                    && x.ProductFk.ProductCode == productCode
                                    && x.ServiceFk.Status == ServiceStatus.Active
                                    && x.CategoryFk.Status == CommonConst.CategoryStatus.Active
                                    && x.ProductFk.Status == CommonConst.ProductStatus.Active
                                    && x.IsOpened && x.ProviderFk.ProviderStatus == CommonConst.ProviderStatus.Active &&
                                    x.CategoryId != null && x.ServiceId != null && x.ProductId != null
                                    && x.UserId == null).WhereIf(masterAccount,
                            x => x.ProviderFk.ParentProvider == null);

                    var item = await query.Select(x => new ServiceConfiguationDto
                    {
                        Description = x.Description,
                        Priority = x.Priority,
                        Name = x.Name,
                        ProductCode = x.ProductFk.ProductCode,
                        CategoryCode = x.CategoryFk.CategoryCode,
                        ProviderCode = x.ProviderFk.Code,
                        ProviderName = x.ProviderFk.Name,
                        IsOpened = x.IsOpened,
                        ServiceCode = x.ServiceFk.ServiceCode,
                        ProductValue = x.ProductFk.ProductValue,
                        TransCodeConfig = x.ProviderFk.TransCodeConfig,

                        //11062022
                        ProviderMaxWaitingTimeout = x.ProviderMaxWaitingTimeout,
                        ProviderSetTransactionTimeout = x.ProviderSetTransactionTimeout,
                        IsEnableResponseWhenJustReceived = x.IsEnableResponseWhenJustReceived,
                        StatusResponseWhenJustReceived = x.StatusResponseWhenJustReceived,
                        IsLastConfiguration = x.IsLastConfiguration,

                        //IsSlowTrans = (x.ProviderFk.IsSlowTrans ?? false) && isApplySlowTrans
                        IsSlowTrans = x.IsEnableResponseWhenJustReceived ?? false,
                        WaitingTimeResponseWhenJustReceived = x.WaitingTimeResponseWhenJustReceived,
                        IsAutoDeposit = x.ProviderFk.IsAutoDeposit,
                        IsRoundRobinAccount = x.ProviderFk.IsRoundRobinAccount,
                        MinBalanceToDeposit = x.ProviderFk.MinBalanceToDeposit,
                        MinBalance = x.ProviderFk.MinBalance,
                        ParentProvider = x.ProviderFk.ParentProvider,
                        DepositAmount = x.ProviderFk.DepositAmount,
                        AllowTopupReceiverType = x.AllowTopupReceiverType,
                        RateRunning = x.RateRunning ?? 0,
                        WorkShortCode = x.ProviderFk.WorkShortCode,
                    }).OrderBy(x => x.Priority).ThenBy(x => x.CategoryCode).ToListAsync();
                    return item.Any() ? item : null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}