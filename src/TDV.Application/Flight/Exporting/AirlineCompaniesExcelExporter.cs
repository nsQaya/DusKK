using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Flight.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Flight.Exporting
{
    public class AirlineCompaniesExcelExporter : NpoiExcelExporterBase, IAirlineCompaniesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AirlineCompaniesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAirlineCompanyForViewDto> airlineCompanies)
        {
            return CreateExcelPackage(
                "AirlineCompanies.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("AirlineCompanies"));

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name"),
                        L("LadingPrefix"),
                        L("FlightPrefix")
                        );

                    AddObjects(
                        sheet, airlineCompanies,
                        _ => _.AirlineCompany.Code,
                        _ => _.AirlineCompany.Name,
                        _ => _.AirlineCompany.LadingPrefix,
                        _ => _.AirlineCompany.FlightPrefix
                        );

                });
        }
    }
}