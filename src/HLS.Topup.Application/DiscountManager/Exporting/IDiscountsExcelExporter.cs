using System.Collections.Generic;
using HLS.Topup.DiscountManager.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Discounts;

namespace HLS.Topup.DiscountManager.Exporting
{
    public interface IDiscountsExcelExporter
    {
        FileDto ExportToFile(List<GetDiscountForViewDto> discounts);

        FileDto DiscountDetailsExportToFile(List<DiscountDetailDto> discounts, string fileName);
    }
}