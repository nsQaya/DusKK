using System.Collections.Generic;
using TDV.Authorization.Users.Dto;
using TDV.Dto;

namespace TDV.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}