using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Location.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Location.Exporting
{
    public class RegionsExcelExporter : NpoiExcelExporterBase, IRegionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RegionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRegionForViewDto> regions)
        {
            return CreateExcelPackage(
                "Regions.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Regions"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Order"),
                        L("IsActive"),
                        (L("FixedPrice")) + L("Name")
                        );

                    AddObjects(
                        sheet, regions,
                        _ => _.Region.Name,
                        _ => _.Region.Order,
                        _ => _.Region.IsActive,
                        _ => _.FixedPriceName
                        );

                });
        }
    }
}