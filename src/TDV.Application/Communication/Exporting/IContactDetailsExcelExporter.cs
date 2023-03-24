using System.Collections.Generic;
using TDV.Communication.Dtos;
using TDV.Dto;

namespace TDV.Communication.Exporting
{
    public interface IContactDetailsExcelExporter
    {
        FileDto ExportToFile(List<GetContactDetailForViewDto> contactDetails);
    }
}