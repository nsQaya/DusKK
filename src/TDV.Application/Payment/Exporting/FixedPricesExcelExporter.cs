using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Payment.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Payment.Exporting
{
    public class FixedPricesExcelExporter : NpoiExcelExporterBase, IFixedPricesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FixedPricesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFixedPriceForViewDto> fixedPrices)
        {
            return CreateExcelPackage(
                "FixedPrices.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("FixedPrices"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description")
                        );

                    AddObjects(
                        sheet, fixedPrices,
                        _ => _.FixedPrice.Name,
                        _ => _.FixedPrice.Description
                        );

                });
        }
    }
}