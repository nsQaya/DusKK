using System.Collections.Generic;
using TDV.Payment.Dtos;
using TDV.Dto;

namespace TDV.Payment.Exporting
{
    public interface IContractFormulesExcelExporter
    {
        FileDto ExportToFile(List<GetContractFormuleForViewDto> contractFormules);
    }
}