using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Location.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Location.Exporting
{
    public class DistrictsExcelExporter : NpoiExcelExporterBase, IDistrictsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DistrictsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDistrictForViewDto> districts)
        {
            return CreateExcelPackage(
                "Districts.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Districts"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Order"),
                        L("IsActive"),
                        (L("City")) + L("DisplayProperty"),
                        (L("Region")) + L("Name")
                        );

                    AddObjects(
                        sheet, districts,
                        _ => _.District.Name,
                        _ => _.District.Order,
                        _ => _.District.IsActive,
                        _ => _.CityDisplayProperty,
                        _ => _.RegionName
                        );

                });
        }
    }
}