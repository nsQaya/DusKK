using System.Collections.Generic;
using TDV.Authorization.Users.Importing.Dto;
using TDV.Dto;

namespace TDV.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
