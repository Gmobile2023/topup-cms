using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.StockManagement.Exporting;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Abp.UI;
using HLS.Topup.Common;
using HLS.Topup.Providers;
using HLS.Topup.RequestDtos;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using HLS.Topup.Dtos.Provider;
using Microsoft.Extensions.Logging;

namespace HLS.Topup.StockManagement
{
    [AbpAuthorize(AppPermissions.Pages_StocksAirtimes)]
    public class StocksAirtimesAppService : TopupAppServiceBase, IStocksAirtimesAppService
    {
        private readonly IStocksAirtimesExcelExporter _stockAirtimeExcelExporter;
        private readonly IRepository<Provider> _lookupProviderRepository;
        private readonly IStockAirtimeManager _stockAirtimeManager;
        private readonly ILogger<StocksAirtimesAppService> _logger;

        public StocksAirtimesAppService(
            IRepository<Provider> lookupProviderRepository,
            IStockAirtimeManager stockAirtimeManager,
            IStocksAirtimesExcelExporter stockAirtimeExcelExporter,
            ILogger<StocksAirtimesAppService> logger)
        {
            _lookupProviderRepository = lookupProviderRepository;
            _stockAirtimeManager = stockAirtimeManager;
            _stockAirtimeExcelExporter = stockAirtimeExcelExporter;
            _logger = logger;
        }

        public async Task<PagedResultDto<StocksAirtimeDto>> GetAll(GetAllStocksAirtimesInput input)
        {
            var request = new GetAllStockAirtimeRequest
            {
                Limit = input.MaxResultCount,
                Offset = input.SkipCount,
                SearchType = SearchType.Search,
                Order = input.Sorting,

                Filter = input.Filter,
                ProviderCode = input.ProviderCodeFilter,
                Status = 99,
            };

            var rs = await _stockAirtimeManager.GetAllStockAirtime(request);

            var totalCount = rs.Total;
            if (rs.ResponseCode != "01")
                return new PagedResultDto<StocksAirtimeDto>(
                    0,
                    new List<StocksAirtimeDto>()
                );
            var data = rs.Payload;
            if (data.Any())
            {
                var providersCode = data.Select(x => x.KeyCode);
                var providers = await _lookupProviderRepository.GetAll()
                    .Where(x => providersCode.Contains(x.Code))
                    .ToListAsync();
                foreach (var item in data)
                {
                    var p = providers.FirstOrDefault(x => x.Code == item.KeyCode);
                    item.ProviderCode = p != null ? p.Code : item.KeyCode;
                    item.ProviderName = p != null ? p.Name : item.KeyCode;
                }
            }

            return new PagedResultDto<StocksAirtimeDto>(
                totalCount, data
            );
        }

        public async Task<StocksAirtimeDto> GetStocksAirtimeForView(string providerCode)
        {
            var response =
                await _stockAirtimeManager.GetStockAirtime(new GetStockAirtimeRequest() {ProviderCode = providerCode});
            var data = response.Payload;
            if (data == null) return null;
            var provider = await _lookupProviderRepository.GetAll()
                .Where(x => data.KeyCode == (x.Code))
                .FirstOrDefaultAsync();
            data.ProviderCode = provider != null ? provider.Code : data.KeyCode;
            data.ProviderName = provider != null ? provider.Name : data.KeyCode;
            return data;
        }

        public async Task<StocksAirtimeDto> GetStocksAirtimeForEdit(string providerCode)
        {
            return await GetStocksAirtimeForView(providerCode);
        }

        public async Task CreateOrEdit(StocksAirtimeDto input)
        {
            if (input.MaxLimitAirtime < input.MinLimitAirtime)
                throw new UserFriendlyException($"Tồn kho tối đã phải lớn hơn tồn kho tối thiểu");
            if (string.IsNullOrEmpty(input.KeyCode))
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_StocksAirtimes_Create)]
        private async Task<ResponseMessages> Create(StocksAirtimeDto input)
        {
            var request = input.ConvertTo<CreateStockAirtimeRequest>();
            var response = await _stockAirtimeManager.GetStockAirtime(new GetStockAirtimeRequest()
                {ProviderCode = input.ProviderCode});
            var data = response.Payload;
            if (data != null)
                throw new UserFriendlyException($"Nhà cung cấp {input.ProviderCode} đã tồn tại kho Airtime!");
            request.Status = 1;
            return await _stockAirtimeManager.CreateStockAirtime(request);
        }

        [AbpAuthorize(AppPermissions.Pages_StocksAirtimes_Edit)]
        private async Task<ResponseMessages> Update(StocksAirtimeDto input)
        {
            var request = input.ConvertTo<UpdateStockAirtimeRequest>();
            request.Status = 1;
            return await _stockAirtimeManager.UpdateStockAirtime(request);
        }

        public async Task<FileDto> GetStocksAirtimesToExcel(GetAllStocksAirtimesForExcelInput input)
        {
            var request = new GetAllStockAirtimeRequest()
            {
                Limit = 0,
                Offset = 0,
                SearchType = SearchType.Export,

                Filter = input.Filter,
                ProviderCode = input.ProviderCodeFilter,
                Status = 99,
            };
            var rs = await _stockAirtimeManager.GetAllStockAirtime(request);
            var data = rs.Payload;
            if (data.Any())
            {
                var providers = await _lookupProviderRepository.GetAll()
                    .ToListAsync();
                foreach (var item in data)
                {
                    var p = providers.FirstOrDefault(x => x.Code == item.KeyCode);
                    item.ProviderCode = p != null ? p.Code : item.KeyCode;
                    item.ProviderName = p != null ? p.Name : item.KeyCode;
                }
            }

            return _stockAirtimeExcelExporter.ExportToFile(data);
        }

        public async Task<List<CommonLookupTableDto>> GetAllProvider()
        {
            return await _lookupProviderRepository.GetAll()
                .Select(p => new CommonLookupTableDto
                {
                    Id = p.Code,
                    DisplayName = p.Name
                }).ToListAsync();
        }

        public async Task<ResponseMessages> Query(string providerCode)
        {
            var request = new GetAvailableStockAirtimeRequest() {ProviderCode = providerCode};
            var response = await _stockAirtimeManager.GetAvailableStockAirtime(request);
            if (response.ResponseStatus.ErrorCode != "01")
                return new ResponseMessages()
                {
                    ResponseCode = response.ResponseStatus.ErrorCode,
                    ResponseMessage = response.ResponseStatus.Message,
                };
            var balance = decimal.Parse(response.Results.ToString(CultureInfo.InvariantCulture)?.Replace(".", ","));
            return new ResponseMessages()
            {
                ResponseCode = "01",
                Payload = balance
            };
        }

        public async Task<ResponseMessages> DepositAirtimeViettel(decimal amount, string providerCode)
        {
            try
            {
                var request = new ViettelDepositRequest() {Amount = amount, ProviderCode = providerCode};
                var response = await _stockAirtimeManager.DepositStockAirtime(request);
                _logger.LogInformation($"DepositAirtimeViettel : {response.ToJson()}");
                if (response.ResponseStatus.ErrorCode != "01")
                    return new ResponseMessages()
                    {
                        ResponseCode = response.ResponseStatus.ErrorCode,
                        ResponseMessage = response.ResponseStatus.Message
                    };
                //var balance = decimal.Parse(response.Results.ToString(CultureInfo.InvariantCulture)?.Replace(".", ","));
                return new ResponseMessages()
                {
                    ResponseCode = "01"
                    //Payload = balance
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"DepositAirtimeViettel : {ex.Message}|{ex.StackTrace}|{ex.InnerException}");
                throw new UserFriendlyException($"Quý khách kiểm tra lại thông tin kết nối.");
            }
        }
    }
}