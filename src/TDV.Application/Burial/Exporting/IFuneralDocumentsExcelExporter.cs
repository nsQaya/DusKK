using System.Collections.Generic;
using TDV.Burial.Dtos;
using TDV.Dto;

namespace TDV.Burial.Exporting
{
    public interface IFuneralDocumentsExcelExporter
    {
        FileDto ExportToFile(List<GetFuneralDocumentForViewDto> funeralDocuments);
    }
}