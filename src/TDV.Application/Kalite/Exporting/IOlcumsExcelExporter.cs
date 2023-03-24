using System.Collections.Generic;
using TDV.Kalite.Dtos;
using TDV.Dto;

namespace TDV.Kalite.Exporting
{
    public interface IOlcumsExcelExporter
    {
        FileDto ExportToFile(List<GetOlcumForViewDto> olcums);
    }
}