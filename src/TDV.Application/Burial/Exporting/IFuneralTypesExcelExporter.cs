using System.Collections.Generic;
using TDV.Burial.Dtos;
using TDV.Dto;

namespace TDV.Burial.Exporting
{
    public interface IFuneralTypesExcelExporter
    {
        FileDto ExportToFile(List<GetFuneralTypeForViewDto> funeralTypes);
    }
}