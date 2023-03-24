using TDV.Burial.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.FuneralFlights
{
    public class CreateOrEditFuneralFlightModalViewModel
    {
        public CreateOrEditFuneralFlightDto FuneralFlight { get; set; }

        public string FuneralName { get; set; }

        public string AirlineCompanyCode { get; set; }

        public string AirportName { get; set; }

        public string AirportName2 { get; set; }

        public bool IsEditMode => FuneralFlight.Id.HasValue;
    }
}