using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Location.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Location.Exporting
{
    public class CountriesExcelExporter : NpoiExcelExporterBase, ICountriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CountriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCountryForViewDto> countries)
        {
            return CreateExcelPackage(
                "Countries.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Countries"));

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name"),
                        L("Order"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, countries,
                        _ => _.Country.Code,
                        _ => _.Country.Name,
                        _ => _.Country.Order,
                        _ => _.Country.IsActive
                        );

                });
        }
    }
}