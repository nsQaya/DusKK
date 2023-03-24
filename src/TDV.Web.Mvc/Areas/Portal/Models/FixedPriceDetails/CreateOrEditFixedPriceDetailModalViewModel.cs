using TDV.Payment.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.FixedPriceDetails
{
    public class CreateOrEditFixedPriceDetailModalViewModel
    {
        public CreateOrEditFixedPriceDetailDto FixedPriceDetail { get; set; }

        public string FixedPriceName { get; set; }

        public bool IsEditMode => FixedPriceDetail.Id.HasValue;
    }
}