using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Burial.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Burial.Exporting
{
    public class FuneralsExcelExporter : NpoiExcelExporterBase, IFuneralsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FuneralsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFuneralForViewDto> funerals)
        {
            return CreateExcelPackage(
                "Funerals.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Funerals"));

                    AddHeader(
                        sheet,
                        L("TransferNo"),
                        L("MemberNo"),
                        L("Name"),
                        L("Surname"),
                        L("TcNo"),
                        L("PassportNo"),
                        L("LadingNo"),
                        L("Status"),
                        L("OperationDate"),
                        (L("FuneralType")) + L("Description"),
                        (L("Contact")) + L("DisplayProperty"),
                        (L("OrganizationUnit")) + L("DisplayName"),
                        (L("OrganizationUnit")) + L("DisplayName"),
                        (L("OrganizationUnit")) + L("DisplayName"),
                        (L("User")) + L("Name"),
                        (L("FuneralPackage")) + L("Code"),
                        (L("Contract")) + L("Formule"),
                        (L("Vehicle")) + L("Plate")
                        );

                    AddObjects(
                        sheet, funerals,
                        _ => _.Funeral.TransferNo,
                        _ => _.Funeral.MemberNo,
                        _ => _.Funeral.Name,
                        _ => _.Funeral.Surname,
                        _ => _.Funeral.TcNo,
                        _ => _.Funeral.PassportNo,
                        _ => _.Funeral.LadingNo,
                        _ => _.Funeral.Status,
                        _ => _timeZoneConverter.Convert(_.Funeral.OperationDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.FuneralTypeDescription,
                        _ => _.ContactDisplayProperty,
                        _ => _.OwnerOrganizationUnitDisplayName,
                        _ => _.GiverOrganizationUnitDisplayName,
                        _ => _.ContractorOrganizationUnitDisplayName,
                        _ => _.UserName,
                        _ => _.FuneralPackageCode,
                        _ => _.ContractFormule,
                        _ => _.VehiclePlate
                        );

                    for (var i = 1; i <= funerals.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[9], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(9);
                });
        }
    }
}