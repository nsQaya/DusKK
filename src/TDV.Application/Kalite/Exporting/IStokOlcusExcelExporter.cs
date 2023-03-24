using System.Collections.Generic;
using TDV.Kalite.Dtos;
using TDV.Dto;

namespace TDV.Kalite.Exporting
{
    public interface IStokOlcusExcelExporter
    {
        FileDto ExportToFile(List<GetStokOlcuForViewDto> stokOlcus);
    }
}