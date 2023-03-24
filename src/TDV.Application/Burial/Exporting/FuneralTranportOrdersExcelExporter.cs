using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TDV.DataExporting.Excel.NPOI;
using TDV.Burial.Dtos;
using TDV.Dto;
using TDV.Storage;

namespace TDV.Burial.Exporting
{
    public class FuneralTranportOrdersExcelExporter : NpoiExcelExporterBase, IFuneralTranportOrdersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FuneralTranportOrdersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFuneralTranportOrderForViewDto> funeralTranportOrders)
        {
            return CreateExcelPackage(
                "FuneralTranportOrders.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("FuneralTranportOrders"));

                    AddHeader(
                        sheet,
                        L("StartDate"),
                        L("StartKM"),
                        L("OperationDate"),
                        L("OperationKM"),
                        L("DeliveryDate"),
                        L("DeliveryKM"),
                        L("EndDate"),
                        L("EndKM"),
                        L("ReceiverFullName"),
                        L("ReceiverKinshipDegree"),
                        (L("FuneralWorkOrderDetail")) + L("Description")
                        );

                    AddObjects(
                        sheet, funeralTranportOrders,
                        _ => _timeZoneConverter.Convert(_.FuneralTranportOrder.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.FuneralTranportOrder.StartKM,
                        _ => _timeZoneConverter.Convert(_.FuneralTranportOrder.OperationDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.FuneralTranportOrder.OperationKM,
                        _ => _timeZoneConverter.Convert(_.FuneralTranportOrder.DeliveryDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.FuneralTranportOrder.DeliveryKM,
                        _ => _timeZoneConverter.Convert(_.FuneralTranportOrder.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.FuneralTranportOrder.EndKM,
                        _ => _.FuneralTranportOrder.ReceiverFullName,
                        _ => _.FuneralTranportOrder.ReceiverKinshipDegree,
                        _ => _.FuneralWorkOrderDetailDescription
                        );

                    for (var i = 1; i <= funeralTranportOrders.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[1], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(1); for (var i = 1; i <= funeralTranportOrders.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3); for (var i = 1; i <= funeralTranportOrders.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[5], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(5); for (var i = 1; i <= funeralTranportOrders.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[7], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(7);
                });
        }
    }
}