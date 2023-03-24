using System.Collections.Generic;
using TDV.Location.Dtos;
using TDV.Dto;

namespace TDV.Location.Exporting
{
    public interface IDistrictsExcelExporter
    {
        FileDto ExportToFile(List<GetDistrictForViewDto> districts);
    }
}