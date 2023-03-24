using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Burial.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Burial.Exporting
{
    public class FuneralTypesExcelExporter : NpoiExcelExporterBase, IFuneralTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FuneralTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFuneralTypeForViewDto> funeralTypes)
        {
            return CreateExcelPackage(
                "FuneralTypes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("FuneralTypes"));

                    AddHeader(
                        sheet,
                        L("Description"),
                        L("IsDefault")
                        );

                    AddObjects(
                        sheet, funeralTypes,
                        _ => _.FuneralType.Description,
                        _ => _.FuneralType.IsDefault
                        );

                });
        }
    }
}