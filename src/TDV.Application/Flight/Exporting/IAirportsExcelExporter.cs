using System.Collections.Generic;
using TDV.Flight.Dtos;
using TDV.Dto;

namespace TDV.Flight.Exporting
{
    public interface IAirportsExcelExporter
    {
        FileDto ExportToFile(List<GetAirportForViewDto> airports);
    }
}