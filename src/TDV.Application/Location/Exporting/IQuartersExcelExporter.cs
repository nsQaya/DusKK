using System.Collections.Generic;
using TDV.Location.Dtos;
using TDV.Dto;

namespace TDV.Location.Exporting
{
    public interface IQuartersExcelExporter
    {
        FileDto ExportToFile(List<GetQuarterForViewDto> quarters);
    }
}