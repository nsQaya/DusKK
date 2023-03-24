using TDV.Burial.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.FuneralFlights
{
    public class MasterDetailChild_Funeral_CreateOrEditFuneralFlightModalViewModel
    {
        public CreateOrEditFuneralFlightDto FuneralFlight { get; set; }

        public string AirlineCompanyCode { get; set; }

        public string AirportName { get; set; }

        public string AirportName2 { get; set; }

        public bool IsEditMode => FuneralFlight.Id.HasValue;
    }
}