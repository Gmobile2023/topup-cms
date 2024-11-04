using Abp.Domain.Repositories;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Configuration;
using HLS.Topup.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HLS.Topup.Configuration
{
    public class PartnerServiceConfigurationManager : TopupDomainServiceBase, IPartnerServiceConfigurationManager
    {
        private readonly IRepository<PartnerServiceConfiguration> _serviceConfigRepository;


        public PartnerServiceConfigurationManager(IRepository<PartnerServiceConfiguration> serviceConfigRepository)
        {
            _serviceConfigRepository = serviceConfigRepository;
        }

        public async Task<List<PartnerServiceConfiguationDto>> GetPartnerServiceConfiguations(string accountCode, string serviceCode,
            string categoryCode)
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
                                    && x.Status == CommonConst.PartnerServiceConfigurationStatus.Active
                                    && x.ProviderFk.ProviderStatus == CommonConst.ProviderStatus.Active
                                    && x.ServiceFk.Status == ServiceStatus.Active
                                    && x.CategoryFk.Status == CommonConst.CategoryStatus.Active
                                    && x.UserFk.AccountCode == accountCode
                                    && x.ServiceId != null
                                    && x.CategoryId != null
                                    && x.UserId != null);

                    var item = await query.Select(x => new PartnerServiceConfiguationDto
                    {
                        Description = x.Description,
                        Name = x.Name,
                        AccountCode = x.UserFk.AccountCode,
                        CategoryCode = x.CategoryFk.CategoryCode,
                        Status = x.Status,
                        ProviderCode = x.ProviderFk.Code,
                        ProviderName = x.ProviderFk.Name,
                        ServiceCode = x.ServiceFk.ServiceCode,
                    }).OrderBy(x => x.Name).ThenBy(x => x.CategoryCode).ToListAsync();
                    return item.Any() ? item : null;
                }
                else
                {
                    var query = _serviceConfigRepository.GetAllIncluding(x => x.ServiceFk)
                        .Include(x => x.CategoryFk)
                        .Include(x => x.ProviderFk)
                        .Where(x => x.CategoryFk.CategoryCode == categoryCode &&
                                    x.ServiceFk.ServiceCode == serviceCode
                                    && x.Status == CommonConst.PartnerServiceConfigurationStatus.Active
                                    && x.ProviderFk.ProviderStatus == CommonConst.ProviderStatus.Active
                                    && x.ServiceFk.Status == ServiceStatus.Active
                                    && x.CategoryFk.Status == CommonConst.CategoryStatus.Active
                                    && x.ServiceId != null && x.CategoryId != null
                                    && x.UserId == null);

                    var item = await query.Select(x => new PartnerServiceConfiguationDto
                    {
                        Description = x.Description,
                        Name = x.Name,
                        CategoryCode = x.CategoryFk.CategoryCode,
                        Status = x.Status,
                        ServiceCode = x.ServiceFk.ServiceCode,
                        ProviderCode = x.ProviderFk.Code,
                        ProviderName = x.ProviderFk.Name
                    }).OrderBy(x => x.Name).ThenBy(x => x.CategoryCode).ToListAsync();
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