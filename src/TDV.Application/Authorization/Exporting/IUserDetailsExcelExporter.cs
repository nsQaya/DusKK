using System.Collections.Generic;
using TDV.Authorization.Dtos;
using TDV.Dto;

namespace TDV.Authorization.Exporting
{
    public interface IUserDetailsExcelExporter
    {
        FileDto ExportToFile(List<GetUserDetailForViewDto> userDetails);
    }
}