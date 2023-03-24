using System.Collections.Generic;
using TDV.Corporation.Dtos;
using TDV.Dto;

namespace TDV.Corporation.Exporting
{
    public interface ICompaniesExcelExporter
    {
        FileDto ExportToFile(List<GetCompanyForViewDto> companies);
    }
}