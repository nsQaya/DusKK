using System.Collections.Generic;
using TDV.Constants.Dtos;
using TDV.Dto;

namespace TDV.Constants.Exporting
{
    public interface IDataListsExcelExporter
    {
        FileDto ExportToFile(List<GetDataListForViewDto> dataLists);
    }
}