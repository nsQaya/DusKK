using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Kalite.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Kalite.Exporting
{
    public class StokOlcusExcelExporter : NpoiExcelExporterBase, IStokOlcusExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StokOlcusExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStokOlcuForViewDto> stokOlcus)
        {
            return CreateExcelPackage(
                    "StokOlcus.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("StokOlcus"));

                        AddHeader(
                            sheet,
                        L("Alt"),
                        L("Ust"),
                        L("Deger"),
                        (L("Stok")) + L("Adi"),
                        (L("Olcum")) + L("OlcuTipi")
                            );

                        AddObjects(
                            sheet, stokOlcus,
                        _ => _.StokOlcu.Alt,
                        _ => _.StokOlcu.Ust,
                        _ => _.StokOlcu.Deger,
                        _ => _.StokAdi,
                        _ => _.OlcumOlcuTipi
                            );

                    });

        }
    }
}