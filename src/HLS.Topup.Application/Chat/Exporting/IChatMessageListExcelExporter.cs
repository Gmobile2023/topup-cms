using System.Collections.Generic;
using Abp;
using HLS.Topup.Chat.Dto;
using HLS.Topup.Dto;

namespace HLS.Topup.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
