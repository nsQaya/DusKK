using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Location.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Location.Exporting
{
    public class CitiesExcelExporter : NpoiExcelExporterBase, ICitiesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CitiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCityForViewDto> cities)
        {
            return CreateExcelPackage(
                "Cities.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Cities"));

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name"),
                        L("Order"),
                        L("IsActive"),
                        (L("Country")) + L("DisplayProperty")
                        );

                    AddObjects(
                        sheet, cities,
                        _ => _.City.Code,
                        _ => _.City.Name,
                        _ => _.City.Order,
                        _ => _.City.IsActive,
                        _ => _.CountryDisplayProperty
                        );

                });
        }
    }
}