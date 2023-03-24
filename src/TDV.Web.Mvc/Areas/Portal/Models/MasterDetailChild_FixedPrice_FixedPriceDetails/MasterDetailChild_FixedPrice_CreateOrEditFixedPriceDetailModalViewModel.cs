using TDV.Payment.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.FixedPriceDetails
{
    public class MasterDetailChild_FixedPrice_CreateOrEditFixedPriceDetailModalViewModel
    {
        public CreateOrEditFixedPriceDetailDto FixedPriceDetail { get; set; }

        public bool IsEditMode => FixedPriceDetail.Id.HasValue;
    }
}