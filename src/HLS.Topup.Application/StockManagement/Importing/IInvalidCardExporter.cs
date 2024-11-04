using System.Collections.Generic;
using Abp.Dependency;
using HLS.Topup.Dto;
using TW.CardMapping.Authorization.Users.Importing.Dto;

namespace HLS.Topup.StockManagement.Importing
{
    public interface IInvalidCardExporter:ITransientDependency
    {
        FileDto ExportToFile(List<ImportCardDto> userListDtos);
    }
}