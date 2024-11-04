using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Providers.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Compare;
using Abp;

namespace HLS.Topup.Providers
{
    public interface ICompareAppService : IApplicationService
    {
        Task<PagedResultDtoReport<CompareDtoReponse>> GetCompareServiceTotalList(
          GetCompareInput input);

        Task<FileDto> GetCompareServiceTotalListToExcel(GetCompareInput input);

        Task<PagedResultDtoReport<CompareRefunDto>> GetCompareRefundList(
         GetCompareRefundInput input);

        Task<PagedResultDtoReport<CompareRefunDetailDto>> GetCompareRefundDetailList(
          GetCompareRefundDetailInput input);

        Task<FileDto> GetCompareRefundDetailListToExcel(GetCompareRefundDetailInput input);
        Task<PagedResultDtoReport<CompareReponseDto>> GetCompareReponseList(
          GetCompareReponseInput input);

        Task<PagedResultDtoReport<CompareReponseDetailDto>> GetCompareReponseDetailList(
          GetCompareReponseDetailInput input);        

        Task<FileDto> GetCompareReponseDetailListToExcel(GetCompareReponseDetailInput input);

        Task<FileDto> GetCompareRefundListToExcel(GetCompareRefundInput input);

        Task<CompareRefunDto> GetCompareRefundSingle(
         GetCompareRefundSingleInput input);

        Task RefundAmountCompare(RefundCompareAmountInput refunDto);

        Task RefundAmoutSelectCompare(RefundCompareSelectInput refunDto);

        Task<ResponseMessages> CheckCompareProviderDate(ReportCheckCompareInput input);

        

        Task ImportCompareJob(CompareProviderRequest data);           
    }
}
