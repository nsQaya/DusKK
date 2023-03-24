using TDV.Corporation.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.Vehicles
{
    public class CreateOrEditVehicleModalViewModel
    {
        public CreateOrEditVehicleDto Vehicle { get; set; }

        public string CompanyDisplayProperty { get; set; }

        public bool IsEditMode => Vehicle.Id.HasValue;
    }
}