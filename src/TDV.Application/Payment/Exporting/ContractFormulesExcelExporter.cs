using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Payment.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Payment.Exporting
{
    public class ContractFormulesExcelExporter : NpoiExcelExporterBase, IContractFormulesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ContractFormulesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetContractFormuleForViewDto> contractFormules)
        {
            return CreateExcelPackage(
                "ContractFormules.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ContractFormules"));

                    AddHeader(
                        sheet,
                        L("Formule"),
                        L("Description")
                        );

                    AddObjects(
                        sheet, contractFormules,
                        _ => _.ContractFormule.Formule,
                        _ => _.ContractFormule.Description
                        );

                });
        }
    }
}