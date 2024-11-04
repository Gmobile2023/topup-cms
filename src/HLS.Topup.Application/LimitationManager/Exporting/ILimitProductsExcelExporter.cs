using System.Collections.Generic;
using HLS.Topup.LimitationManager.Dtos;
using HLS.Topup.Dto;

namespace HLS.Topup.LimitationManager.Exporting
{
    public interface ILimitProductsExcelExporter
    {
        FileDto ExportToFile(List<GetLimitProductForViewDto> limitProducts, string fileName);

        FileDto DetailLimitExportToFile(List<LimitProductDetailDto> limitProductsDetail, string fileName);
    }
}