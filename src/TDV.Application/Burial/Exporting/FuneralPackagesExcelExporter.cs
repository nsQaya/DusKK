using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Burial.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Burial.Exporting
{
    public class FuneralPackagesExcelExporter : NpoiExcelExporterBase, IFuneralPackagesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FuneralPackagesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFuneralPackageForViewDto> funeralPackages)
        {
            return CreateExcelPackage(
                "FuneralPackages.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("FuneralPackages"));

                    AddHeader(
                        sheet,
                        L("Status"),
                        L("Code"),
                        L("Description")
                        );

                    AddObjects(
                        sheet, funeralPackages,
                        _ => _.FuneralPackage.Status,
                        _ => _.FuneralPackage.Code,
                        _ => _.FuneralPackage.Description
                        );

                });
        }
    }
}