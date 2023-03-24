using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Flight.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Flight.Exporting
{
    public class AirportsExcelExporter : NpoiExcelExporterBase, IAirportsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AirportsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAirportForViewDto> airports)
        {
            return CreateExcelPackage(
                "Airports.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Airports"));

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name"),
                        L("Description"),
                        L("Order"),
                        L("IsActive"),
                        (L("Country")) + L("DisplayProperty"),
                        (L("City")) + L("DisplayProperty")
                        );

                    AddObjects(
                        sheet, airports,
                        _ => _.Airport.Code,
                        _ => _.Airport.Name,
                        _ => _.Airport.Description,
                        _ => _.Airport.Order,
                        _ => _.Airport.IsActive,
                        _ => _.CountryDisplayProperty,
                        _ => _.CityDisplayProperty
                        );

                });
        }
    }
}