using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Burial.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Burial.Exporting
{
    public class FuneralAddresesExcelExporter : NpoiExcelExporterBase, IFuneralAddresesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FuneralAddresesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFuneralAddresForViewDto> funeralAddreses)
        {
            return CreateExcelPackage(
                "FuneralAddreses.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("FuneralAddreses"));

                    AddHeader(
                        sheet,
                        L("Description"),
                        L("Address"),
                        (L("Funeral")) + L("DisplayProperty"),
                        (L("Quarter")) + L("Name")
                        );

                    AddObjects(
                        sheet, funeralAddreses,
                        _ => _.FuneralAddres.Description,
                        _ => _.FuneralAddres.Address,
                        _ => _.FuneralDisplayProperty,
                        _ => _.QuarterName
                        );

                });
        }
    }
}