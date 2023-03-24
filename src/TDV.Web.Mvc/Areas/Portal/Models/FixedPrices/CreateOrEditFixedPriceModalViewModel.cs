using TDV.Payment.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.FixedPrices
{
    public class CreateOrEditFixedPriceModalViewModel
    {
        public CreateOrEditFixedPriceDto FixedPrice { get; set; }

        public bool IsEditMode => FixedPrice.Id.HasValue;
    }
}