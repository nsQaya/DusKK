using System.Collections.Generic;
using TDV.Corporation.Dtos;
using TDV.Dto;

namespace TDV.Corporation.Exporting
{
    public interface IVehiclesExcelExporter
    {
        FileDto ExportToFile(List<GetVehicleForViewDto> vehicles);
    }
}