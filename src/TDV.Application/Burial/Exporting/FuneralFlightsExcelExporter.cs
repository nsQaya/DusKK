using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Burial.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Burial.Exporting
{
    public class FuneralFlightsExcelExporter : NpoiExcelExporterBase, IFuneralFlightsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FuneralFlightsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFuneralFlightForViewDto> funeralFlights)
        {
            return CreateExcelPackage(
                "FuneralFlights.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("FuneralFlights"));

                    AddHeader(
                        sheet,
                        L("No"),
                        L("Code"),
                        L("LiftOffDate"),
                        L("LandingDate"),
                        (L("Funeral")) + L("Name"),
                        (L("AirlineCompany")) + L("Code"),
                        (L("Airport")) + L("Name"),
                        (L("Airport")) + L("Name")
                        );

                    AddObjects(
                        sheet, funeralFlights,
                        _ => _.FuneralFlight.No,
                        _ => _.FuneralFlight.Code,
                        _ => _timeZoneConverter.Convert(_.FuneralFlight.LiftOffDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.FuneralFlight.LandingDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.FuneralName,
                        _ => _.AirlineCompanyCode,
                        _ => _.AirportName,
                        _ => _.AirportName2
                        );

                    for (var i = 1; i <= funeralFlights.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3); for (var i = 1; i <= funeralFlights.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(4);
                });
        }
    }
}