using System.Collections.Generic;
using TDV.Communication.Dtos;
using TDV.Dto;

namespace TDV.Communication.Exporting
{
    public interface IContactsExcelExporter
    {
        FileDto ExportToFile(List<GetContactForViewDto> contacts);
    }
}