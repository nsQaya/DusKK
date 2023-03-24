using System.Collections.Generic;
using TDV.Constants.Dtos;
using TDV.Dto;

namespace TDV.Constants.Exporting
{
    public interface ICurrenciesExcelExporter
    {
        FileDto ExportToFile(List<GetCurrencyForViewDto> currencies);
    }
}