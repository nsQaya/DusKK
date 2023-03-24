using System.Collections.Generic;
using TDV.Burial.Dtos;
using TDV.Dto;

namespace TDV.Burial.Exporting
{
    public interface IFuneralsExcelExporter
    {
        FileDto ExportToFile(List<GetFuneralForViewDto> funerals);
    }
}