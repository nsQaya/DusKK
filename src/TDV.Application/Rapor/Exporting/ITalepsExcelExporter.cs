using System.Collections.Generic;
using TDV.Rapor.Dtos;
using TDV.Dto;

namespace TDV.Rapor.Exporting
{
    public interface ITalepsExcelExporter
    {
        FileDto ExportToFile(List<GetTalepForViewDto> taleps);
    }
}