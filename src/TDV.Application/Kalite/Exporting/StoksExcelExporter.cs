using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Kalite.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Kalite.Exporting
{
    public class StoksExcelExporter : NpoiExcelExporterBase, IStoksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStokForViewDto> stoks)
        {
            return CreateExcelPackage(
                    "Stoks.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("Stoks"));

                        AddHeader(
                            sheet,
                        L("Kodu"),
                        L("Adi")
                            );

                        AddObjects(
                            sheet, stoks,
                        _ => _.Stok.Kodu,
                        _ => _.Stok.Adi
                            );

                    });

        }
    }
}