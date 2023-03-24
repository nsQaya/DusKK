using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Rapor.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Rapor.Exporting
{
    public class TalepsExcelExporter : NpoiExcelExporterBase, ITalepsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TalepsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTalepForViewDto> taleps)
        {
            return CreateExcelPackage(
                    "Taleps.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("Taleps"));

                        AddHeader(
                            sheet,
                        L("TalepMiktar"),
                        L("OlcuBr"),
                        L("Fiyat"),
                        L("Tutar"),
                        (L("Stok")) + L("Adi")
                            );

                        AddObjects(
                            sheet, taleps,
                        _ => _.Talep.TalepMiktar,
                        _ => _.Talep.OlcuBr,
                        _ => _.Talep.Fiyat,
                        _ => _.Talep.Tutar,
                        _ => _.StokAdi
                            );

                    });

        }
    }
}