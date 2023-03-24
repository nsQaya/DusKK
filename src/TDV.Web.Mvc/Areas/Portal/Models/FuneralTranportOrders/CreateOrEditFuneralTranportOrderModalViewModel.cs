using TDV.Burial.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.FuneralTranportOrders
{
    public class CreateOrEditFuneralTranportOrderModalViewModel
    {
        public CreateOrEditFuneralTranportOrderDto FuneralTranportOrder { get; set; }

        public string FuneralWorkOrderDetailDescription { get; set; }

        public bool IsEditMode => FuneralTranportOrder.Id.HasValue;
    }
}