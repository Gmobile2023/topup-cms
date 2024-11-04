using System.Collections.Generic;
using HLS.Topup.Products.Dtos;
using HLS.Topup.Dto;

namespace HLS.Topup.Products.Exporting
{
    public interface IProductsExcelExporter
    {
        FileDto ExportToFile(List<GetProductForViewDto> products);
    }
}