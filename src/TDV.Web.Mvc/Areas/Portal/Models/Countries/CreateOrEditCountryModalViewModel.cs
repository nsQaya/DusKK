using TDV.Location.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.Countries
{
    public class CreateOrEditCountryModalViewModel
    {
        public CreateOrEditCountryDto Country { get; set; }

        public bool IsEditMode => Country.Id.HasValue;
    }
}