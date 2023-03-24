using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Burial.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Burial.Exporting
{
    public class FuneralDocumentsExcelExporter : NpoiExcelExporterBase, IFuneralDocumentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FuneralDocumentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFuneralDocumentForViewDto> funeralDocuments)
        {
            return CreateExcelPackage(
                    "FuneralDocuments.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("FuneralDocuments"));

                        AddHeader(
                            sheet,
                        L("Type"),
                        L("Path"),
                        L("Guid"),
                        (L("Funeral")) + L("DisplayProperty")
                            );

                        AddObjects(
                            sheet, funeralDocuments,
                        _ => _.FuneralDocument.Type,
                        _ => _.FuneralDocument.Path,
                        _ => _.FuneralDocument.Guid,
                        _ => _.FuneralDisplayProperty
                            );

                    });

        }
    }
}