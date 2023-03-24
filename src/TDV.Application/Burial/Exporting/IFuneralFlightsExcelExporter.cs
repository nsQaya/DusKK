using System.Collections.Generic;
using TDV.Burial.Dtos;
using TDV.Dto;

namespace TDV.Burial.Exporting
{
    public interface IFuneralFlightsExcelExporter
    {
        FileDto ExportToFile(List<GetFuneralFlightForViewDto> funeralFlights);
    }
}