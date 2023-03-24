using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Corporation.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Corporation.Exporting
{
    public class CompanyContactsExcelExporter : NpoiExcelExporterBase, ICompanyContactsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CompanyContactsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCompanyContactForViewDto> companyContacts)
        {
            return CreateExcelPackage(
                "CompanyContacts.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("CompanyContacts"));

                    AddHeader(
                        sheet,
                        L("Title"),
                        (L("Company")) + L("DisplayProperty"),
                        (L("Contact")) + L("Name")
                        );

                    AddObjects(
                        sheet, companyContacts,
                        _ => _.CompanyContact.Title,
                        _ => _.CompanyDisplayProperty,
                        _ => _.ContactName
                        );

                });
        }
    }
}