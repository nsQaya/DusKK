using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Corporation.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Corporation.Exporting
{
    public class VehiclesExcelExporter : NpoiExcelExporterBase, IVehiclesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public VehiclesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetVehicleForViewDto> vehicles)
        {
            return CreateExcelPackage(
                "Vehicles.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Vehicles"));

                    AddHeader(
                        sheet,
                        L("Plate"),
                        L("Description"),
                        L("EndExaminationDate"),
                        L("EndInsuranceDate"),
                        L("EndGuarantyDate"),
                        L("Capactiy"),
                        L("Year"),
                        L("Brand"),
                        L("TrackNo"),
                        (L("Company")) + L("DisplayProperty")
                        );

                    AddObjects(
                        sheet, vehicles,
                        _ => _.Vehicle.Plate,
                        _ => _.Vehicle.Description,
                        _ => _timeZoneConverter.Convert(_.Vehicle.EndExaminationDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Vehicle.EndInsuranceDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Vehicle.EndGuarantyDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Vehicle.Capactiy,
                        _ => _.Vehicle.Year,
                        _ => _.Vehicle.Brand,
                        _ => _.Vehicle.TrackNo,
                        _ => _.CompanyDisplayProperty
                        );

                    for (var i = 1; i <= vehicles.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3); for (var i = 1; i <= vehicles.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(4); for (var i = 1; i <= vehicles.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[5], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(5);
                });
        }
    }
}