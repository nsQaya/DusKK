using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Kalite.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Kalite.Exporting
{
    public class OlcumsExcelExporter : NpoiExcelExporterBase, IOlcumsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OlcumsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOlcumForViewDto> olcums)
        {
            return CreateExcelPackage(
                    "Olcums.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("Olcums"));

                        AddHeader(
                            sheet,
                        L("OlcuTipi")
                            );

                        AddObjects(
                            sheet, olcums,
                        _ => _.Olcum.OlcuTipi
                            );

                    });

        }
    }
}