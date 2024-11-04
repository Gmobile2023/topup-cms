using HLS.Topup.StockManagement;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
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
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Providers;
using HLS.Topup.RequestDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ServiceStack;

namespace HLS.Topup.StockManagement
{
    [AbpAuthorize(AppPermissions.Pages_BatchAirtimes)]
    public class BatchAirtimesAppService : TopupAppServiceBase, IBatchAirtimesAppService
    {
        private readonly IRepository<Provider> _lookupProviderRepository;
        private readonly IStockAirtimeManager _stockAirtimeManager;
        private readonly IBatchAirtimesExcelExporter _batchAirtimesExcelExporter;
        private readonly IRepository<User, long> _userRepository;


        public BatchAirtimesAppService(IRepository<Provider> lookupProviderRepository,
            IStockAirtimeManager stockAirtimeManager,
            IRepository<User, long> userRepository,
            IBatchAirtimesExcelExporter batchAirtimesExcelExporter)
        {
            _lookupProviderRepository = lookupProviderRepository;
            _stockAirtimeManager = stockAirtimeManager;
            _batchAirtimesExcelExporter = batchAirtimesExcelExporter;
            _userRepository = userRepository;
        }

        public async Task<PagedResultDto<BatchAirtimeDto>> GetAll(GetAllBatchAirtimesInput input)
        {
            var request = new GetAllBatchAirtimeRequest
            {
                Limit = input.MaxResultCount,
                Offset = input.SkipCount,
                SearchType = SearchType.Search,
                Order = input.Sorting,

                Filter = input.Filter,
                BatchCode = input.BatchCodeFilter,
                ProviderCode = input.ProviderCodeFilter,
                Status = (byte) input.StatusFilter,
                FormDate = input.FormDate,
                ToDate = input.ToDate,
            };

            var rs = await _stockAirtimeManager.GetAllBatchAirtime(request);

            var totalCount = rs.Total;
            if (rs.ResponseCode != "01")
                return new PagedResultDto<BatchAirtimeDto>(
                    0,
                    new List<BatchAirtimeDto>()
                );
            var data = rs.Payload;
            if (data.Any())
            {
                var providersCode = data.Select(x => x.ProviderCode);
                var providers = await _lookupProviderRepository.GetAll()
                    .Where(x => providersCode.Contains(x.Code))
                    .ToListAsync();
                
                var createdAccount = data.Select(x => x.CreatedAccount).Distinct();
                var modifiedAccount = data.Select(x => x.ModifiedAccount).Distinct();
                var accounts =   await _userRepository.GetAll()
                    .Where(x => createdAccount.Contains(x.AccountCode) || modifiedAccount.Contains(x.AccountCode))
                    .ToListAsync();
                 
                foreach (var item in data)
                {
                    var p = providers.FirstOrDefault(x => x.Code == item.ProviderCode);
                    item.ProviderCode = p != null ? p.Code : item.ProviderCode;
                    item.ProviderName = p != null ? p.Name : item.ProviderCode;
                    if (!string.IsNullOrEmpty(item.CreatedAccount))
                    {
                        var user = accounts.FirstOrDefault(x => x.AccountCode == item.CreatedAccount);
                        item.CreatedAccountName = user != null ? (user.UserName + " - " + user.FullName ) : item.CreatedAccount;
                    }
                    if (!string.IsNullOrEmpty(item.ModifiedAccount))
                    {
                        var user = accounts.FirstOrDefault(x => x.AccountCode == item.ModifiedAccount);
                        item.ModifiedAccountName = user != null ? (user.UserName + " - " + user.FullName ) : item.ModifiedAccount;
                    }
                }
            }  
            return new PagedResultDto<BatchAirtimeDto>(
                totalCount, rs.Payload.ConvertTo<List<BatchAirtimeDto>>()
            );
        }

        public async Task<BatchAirtimeDto> GetBatchAirtimeForView(string batchCode)
        {
           return await GetBatchAirtimeForEdit(batchCode);
        }

        public async Task<BatchAirtimeDto> GetBatchAirtimeForEdit(string batchCode)
        {
            var response =
                await _stockAirtimeManager.GetBatchAirtime(new GetBatchAirtimeRequest() {BatchCode = batchCode});
            if (response.ResponseCode != "01")
                return null;
            var data = response.Payload;
            var provider = await _lookupProviderRepository.GetAll()
                .Where(x => x.Code == data.ProviderCode).FirstOrDefaultAsync();
            data.ProviderName = provider.Name;
            return response.Payload;
        }

        public async Task CreateOrEdit(BatchAirtimeDto input)
        {
            if (string.IsNullOrEmpty(input.BatchCode))
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_BatchAirtimes_Create)]
        private async Task<ResponseMessages> Create(BatchAirtimeDto input)
        {
            var request = input.ConvertTo<CreateBatchAirtimeRequest>(); 
            return await _stockAirtimeManager.CreateBatchAirtime(request);
        }

        [AbpAuthorize(AppPermissions.Pages_BatchAirtimes_Edit)]
        private async Task<ResponseMessages> Update(BatchAirtimeDto input)
        {
            var request = input.ConvertTo<UpdateBatchAirtimeRequest>();
            return await _stockAirtimeManager.UpdateBatchAirtime(request);
        }

        [AbpAuthorize(AppPermissions.Pages_BatchAirtimes_Approval)]
        public async Task Approval(string code)
        {
            var response =
                await _stockAirtimeManager.GetBatchAirtime(new GetBatchAirtimeRequest() {BatchCode = code});
            var data = response.Payload;
            data.Status = (BatchAirtimeStatus.Approval);
            await _stockAirtimeManager.UpdateBatchAirtime(data.ConvertTo<UpdateBatchAirtimeRequest>());
        }

        [AbpAuthorize(AppPermissions.Pages_BatchAirtimes_Approval)]
        public async Task Reject(string code)
        {
            var response =
                await _stockAirtimeManager.GetBatchAirtime(new GetBatchAirtimeRequest() {BatchCode = code});
            var data = response.Payload;
            data.Status = (BatchAirtimeStatus.Reject);
            await _stockAirtimeManager.UpdateBatchAirtime(data.ConvertTo<UpdateBatchAirtimeRequest>());
        }
        [AbpAuthorize(AppPermissions.Pages_BatchAirtimes_Delete)]
        public async Task Delete(string code)
        {
            var response =
                await _stockAirtimeManager.GetBatchAirtime(new GetBatchAirtimeRequest() {BatchCode = code});
            if (response.ResponseCode != "01")
                throw new UserFriendlyException(response.ResponseMessage);
            var data = response.Payload;
            await _stockAirtimeManager.DateteBatchAirtime(data.ConvertTo<DeleteBatchAirtimeRequest>());
        }
        
        public async Task<FileDto> GetBatchAirtimesToExcel(GetAllBatchAirtimesForExcelInput input)
        {
            var request = new GetAllBatchAirtimeRequest()
            {
                Limit = 0,
                Offset = 0,
                SearchType = SearchType.Export,

                Filter = input.Filter,
                BatchCode = input.BatchCodeFilter,
                ProviderCode = input.ProviderCodeFilter,
                Status = (byte) input.StatusFilter,
                FormDate = input.FormDate,
                ToDate = input.ToDate,
            };
            var rs = await _stockAirtimeManager.GetAllBatchAirtime(request); 
            var data = rs.Payload;
            if (data.Any())
            { 
                var providers = await _lookupProviderRepository.GetAll() 
                    .ToListAsync();
                foreach (var item in data)
                {
                    var p = providers.FirstOrDefault(x => x.Code == item.ProviderCode);
                    item.ProviderCode = p != null ? p.Code : item.ProviderCode;
                    item.ProviderName = p != null ? p.Name : item.ProviderCode; 
                }
            }
            
            return _batchAirtimesExcelExporter.ExportToFile(data);
        }
    }
}