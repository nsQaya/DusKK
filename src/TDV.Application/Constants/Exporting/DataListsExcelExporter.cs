using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Constants.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Constants.Exporting
{
    public class DataListsExcelExporter : NpoiExcelExporterBase, IDataListsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DataListsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDataListForViewDto> dataLists)
        {
            return CreateExcelPackage(
                "DataLists.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("DataLists"));

                    AddHeader(
                        sheet,
                        L("Type"),
                        L("Value"),
                        L("OrderNumber"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, dataLists,
                        _ => _.DataList.Type,
                        _ => _.DataList.Value,
                        _ => _.DataList.OrderNumber,
                        _ => _.DataList.IsActive
                        );

                });
        }
    }
}