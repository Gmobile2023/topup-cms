using System.Collections.Generic;
using HLS.Topup.Providers.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Compare;

namespace HLS.Topup.Providers.Exporting
{
    public interface IProvidersExcelExporter
    {
        FileDto ExportToFile(List<GetProviderForViewDto> providers);

        FileDto ExportCompareToFile(List<CompareDtoReponse> input);

        FileDto ExportCompareDetailToFile(List<CompareReponseDetailDto> input);

        FileDto ExportCompareRefundToFile(List<CompareRefunDto> input);

        FileDto ExportCompareRefundDetailToFile(List<CompareRefunDetailDto> input);
    }
}