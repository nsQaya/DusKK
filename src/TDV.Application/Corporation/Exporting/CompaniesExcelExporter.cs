using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Corporation.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Corporation.Exporting
{
    public class CompaniesExcelExporter : NpoiExcelExporterBase, ICompaniesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CompaniesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCompanyForViewDto> companies)
        {
            return CreateExcelPackage(
                "Companies.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Companies"));

                    AddHeader(
                        sheet,
                        L("IsActive"),
                        L("Type"),
                        L("TaxAdministration"),
                        L("TaxNo"),
                        L("Website"),
                        L("Phone"),
                        L("Fax"),
                        L("Email"),
                        L("Address"),
                        L("RunningCode"),
                        L("Prefix"),
                        (L("OrganizationUnit")) + L("DisplayName"),
                        (L("City")) + L("DisplayProperty"),
                        (L("Quarter")) + L("Name")
                        );

                    AddObjects(
                        sheet, companies,
                        _ => _.Company.IsActive,
                        _ => _.Company.Type,
                        _ => _.Company.TaxAdministration,
                        _ => _.Company.TaxNo,
                        _ => _.Company.Website,
                        _ => _.Company.Phone,
                        _ => _.Company.Fax,
                        _ => _.Company.Email,
                        _ => _.Company.Address,
                        _ => _.Company.RunningCode,
                        _ => _.Company.Prefix,
                        _ => _.OrganizationUnitDisplayName,
                        _ => _.CityDisplayProperty,
                        _ => _.QuarterName
                        );

                });
        }
    }
}