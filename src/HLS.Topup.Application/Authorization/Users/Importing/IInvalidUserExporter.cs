using System.Collections.Generic;
using HLS.Topup.Authorization.Users.Importing.Dto;
using HLS.Topup.Dto;

namespace HLS.Topup.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
