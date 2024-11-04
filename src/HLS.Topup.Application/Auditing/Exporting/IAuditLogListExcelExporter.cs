using System.Collections.Generic;
using HLS.Topup.Auditing.Dto;
using HLS.Topup.Dto;

namespace HLS.Topup.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
