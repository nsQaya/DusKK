using System.Collections.Generic;
using TDV.Burial.Dtos;
using TDV.Dto;

namespace TDV.Burial.Exporting
{
    public interface IFuneralPackagesExcelExporter
    {
        FileDto ExportToFile(List<GetFuneralPackageForViewDto> funeralPackages);
    }
}