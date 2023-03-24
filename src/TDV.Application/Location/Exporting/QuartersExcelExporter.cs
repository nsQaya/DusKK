using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Location.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Location.Exporting
{
    public class QuartersExcelExporter : NpoiExcelExporterBase, IQuartersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public QuartersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetQuarterForViewDto> quarters)
        {
            return CreateExcelPackage(
                "Quarters.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Quarters"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Order"),
                        L("IsActive"),
                        (L("District")) + L("Name")
                        );

                    AddObjects(
                        sheet, quarters,
                        _ => _.Quarter.Name,
                        _ => _.Quarter.Order,
                        _ => _.Quarter.IsActive,
                        _ => _.DistrictName
                        );

                });
        }
    }
}