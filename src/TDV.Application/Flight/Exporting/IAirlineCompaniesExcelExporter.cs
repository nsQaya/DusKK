using System.Collections.Generic;
using TDV.Flight.Dtos;
using TDV.Dto;

namespace TDV.Flight.Exporting
{
    public interface IAirlineCompaniesExcelExporter
    {
        FileDto ExportToFile(List<GetAirlineCompanyForViewDto> airlineCompanies);
    }
}