using System.Collections.Generic;
using TDV.Location.Dtos;
using TDV.Dto;

namespace TDV.Location.Exporting
{
    public interface ICitiesExcelExporter
    {
        FileDto ExportToFile(List<GetCityForViewDto> cities);
    }
}