using System.Collections.Generic;
using HLS.Topup.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace HLS.Topup.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
