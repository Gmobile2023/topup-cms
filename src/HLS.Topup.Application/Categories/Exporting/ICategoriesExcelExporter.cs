using System.Collections.Generic;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Dto;

namespace HLS.Topup.Categories.Exporting
{
    public interface ICategoriesExcelExporter
    {
        FileDto ExportToFile(List<GetCategoryForViewDto> categories);
    }
}