using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Authorization.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Authorization.Exporting
{
    public class UserDetailsExcelExporter : NpoiExcelExporterBase, IUserDetailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public UserDetailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetUserDetailForViewDto> userDetails)
        {
            return CreateExcelPackage(
                "UserDetails.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("UserDetails"));

                    AddHeader(
                        sheet,
                        (L("User")) + L("Name"),
                        (L("Contact")) + L("DisplayProperty")
                        );

                    AddObjects(
                        sheet, userDetails,
                        _ => _.UserName,
                        _ => _.ContactDisplayProperty
                        );

                });
        }
    }
}