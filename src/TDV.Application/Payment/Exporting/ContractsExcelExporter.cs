using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Payment.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Payment.Exporting
{
    public class ContractsExcelExporter : NpoiExcelExporterBase, IContractsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ContractsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetContractForViewDto> contracts)
        {
            return CreateExcelPackage(
                "Contracts.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Contracts"));

                    AddHeader(
                        sheet,
                        L("Formule"),
                        L("StartDate"),
                        L("EndDate"),
                        L("CurrencyType"),
                        (L("Region")) + L("Name"),
                        (L("Company")) + L("DisplayProperty")
                        );

                    AddObjects(
                        sheet, contracts,
                        _ => _.Contract.Formule,
                        _ => _timeZoneConverter.Convert(_.Contract.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Contract.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Contract.CurrencyType,
                        _ => _.RegionName,
                        _ => _.CompanyDisplayProperty
                        );

                    for (var i = 1; i <= contracts.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(2); for (var i = 1; i <= contracts.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3);
                });
        }
    }
}