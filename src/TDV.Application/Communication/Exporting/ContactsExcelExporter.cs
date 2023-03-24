using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Communication.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Communication.Exporting
{
    public class ContactsExcelExporter : NpoiExcelExporterBase, IContactsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ContactsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetContactForViewDto> contacts)
        {
            return CreateExcelPackage(
                "Contacts.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Contacts"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Surname"),
                        L("IdentifyNo")
                        );

                    AddObjects(
                        sheet, contacts,
                        _ => _.Contact.Name,
                        _ => _.Contact.Surname,
                        _ => _.Contact.IdentifyNo
                        );

                });
        }
    }
}