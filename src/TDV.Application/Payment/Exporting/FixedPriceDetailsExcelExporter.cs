using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Payment.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Payment.Exporting
{
    public class FixedPriceDetailsExcelExporter : NpoiExcelExporterBase, IFixedPriceDetailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FixedPriceDetailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFixedPriceDetailForViewDto> fixedPriceDetails)
        {
            return CreateExcelPackage(
                "FixedPriceDetails.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("FixedPriceDetails"));

                    AddHeader(
                        sheet,
                        L("Type"),
                        L("StartDate"),
                        L("EndDate"),
                        L("CurrencyType"),
                        L("Price"),
                        (L("FixedPrice")) + L("Name")
                        );

                    AddObjects(
                        sheet, fixedPriceDetails,
                        _ => _.FixedPriceDetail.Type,
                        _ => _timeZoneConverter.Convert(_.FixedPriceDetail.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FixedPriceDetail.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.FixedPriceDetail.CurrencyType,
                        _ => _.FixedPriceDetail.Price,
                        _ => _.FixedPriceName
                        );

                    for (var i = 1; i <= fixedPriceDetails.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(2); for (var i = 1; i <= fixedPriceDetails.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3);
                });
        }
    }
}