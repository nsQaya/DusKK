using System.Collections.Generic;
using TDV.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace TDV.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
