using TDV.Constants.Dtos;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.Currencies
{
    public class CreateOrEditCurrencyModalViewModel
    {
        public CreateOrEditCurrencyDto Currency { get; set; }

        public bool IsEditMode => Currency.Id.HasValue;
    }
}