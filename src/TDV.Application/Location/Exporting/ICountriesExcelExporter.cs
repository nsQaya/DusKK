using System.Collections.Generic;
using TDV.Location.Dtos;
using TDV.Dto;

namespace TDV.Location.Exporting
{
    public interface ICountriesExcelExporter
    {
        FileDto ExportToFile(List<GetCountryForViewDto> countries);
    }
}