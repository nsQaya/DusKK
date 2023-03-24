using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Communication.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Communication.Exporting
{
    public class ContactDetailsExcelExporter : NpoiExcelExporterBase, IContactDetailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ContactDetailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetContactDetailForViewDto> contactDetails)
        {
            return CreateExcelPackage(
                "ContactDetails.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ContactDetails"));

                    AddHeader(
                        sheet,
                        L("Type"),
                        L("Value"),
                        (L("Contact")) + L("IdentifyNo")
                        );

                    AddObjects(
                        sheet, contactDetails,
                        _ => _.ContactDetail.Type,
                        _ => _.ContactDetail.Value,
                        _ => _.ContactIdentifyNo
                        );

                });
        }
    }
}