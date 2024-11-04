using System.Collections.Generic;
using HLS.Topup.Authorization.Users.Dto;
using HLS.Topup.Dto;

namespace HLS.Topup.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}