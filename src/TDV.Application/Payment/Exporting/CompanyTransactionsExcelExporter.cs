using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Payment.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Payment.Exporting
{
    public class CompanyTransactionsExcelExporter : NpoiExcelExporterBase, ICompanyTransactionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CompanyTransactionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCompanyTransactionForViewDto> companyTransactions)
        {
            return CreateExcelPackage(
                    "CompanyTransactions.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("CompanyTransactions"));

                        AddHeader(
                            sheet,
                        L("InOut"),
                        L("Date"),
                        L("No"),
                        L("Description"),
                        L("Amount"),
                        L("Price"),
                        L("TaxRate"),
                        L("Total"),
                        L("IsTransferred"),
                        (L("Company")) + L("TaxAdministration"),
                        (L("Funeral")) + L("DisplayProperty"),
                        (L("DataList")) + L("Value"),
                        (L("Currency")) + L("Code"),
                        (L("DataList")) + L("Value")
                            );

                        AddObjects(
                            sheet, companyTransactions,
                        _ => _.CompanyTransaction.InOut,
                        _ => _timeZoneConverter.Convert(_.CompanyTransaction.Date, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.CompanyTransaction.No,
                        _ => _.CompanyTransaction.Description,
                        _ => _.CompanyTransaction.Amount,
                        _ => _.CompanyTransaction.Price,
                        _ => _.CompanyTransaction.TaxRate,
                        _ => _.CompanyTransaction.Total,
                        _ => _.CompanyTransaction.IsTransferred,
                        _ => _.CompanyTaxAdministration,
                        _ => _.FuneralDisplayProperty,
                        _ => _.DataListValue,
                        _ => _.CurrencyCode,
                        _ => _.DataListValue2
                            );

                        for (var i = 1; i <= companyTransactions.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[2 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(2 - 1);
                    });

        }
    }
}