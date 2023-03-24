using TDV.Flight.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.AirlineCompanies
{
    public class CreateOrEditAirlineCompanyModalViewModel
    {
        public CreateOrEditAirlineCompanyDto AirlineCompany { get; set; }

        public bool IsEditMode => AirlineCompany.Id.HasValue;
    }
}