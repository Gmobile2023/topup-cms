using System.Collections.Generic;
using HLS.Topup.Dto;
using HLS.Topup.PayBacks.Dtos;

namespace HLS.Topup.PayBacks.Exporting
{
    public interface IPayBacksExcelExporter
    {
        FileDto ExportToFile(List<GetPayBackForViewDto> payBacks);

        FileDto DetailPayBacksExportToFile(List<PayBacksDetailDto> payBacksDetail, string fileName);
    }
}