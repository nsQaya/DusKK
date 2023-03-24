using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Constants.Dtos;
using TDV.Dto;
using TDV.Storage;
using TDV.Constants.Dtos;

namespace TDV.Constants.Exporting
{
    public class CurrenciesExcelExporter : NpoiExcelExporterBase, ICurrenciesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CurrenciesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCurrencyForViewDto> currencies)
        {
            return CreateExcelPackage(
                "Currencies.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Currencies"));

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Symbol"),
                        L("OrderNumber"),
                        L("NumberCode"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, currencies,
                        _ => _.Currency.Code,
                        _ => _.Currency.Symbol,
                        _ => _.Currency.OrderNumber,
                        _ => _.Currency.NumberCode,
                        _ => _.Currency.IsActive
                        );

                });
        }
    }
}