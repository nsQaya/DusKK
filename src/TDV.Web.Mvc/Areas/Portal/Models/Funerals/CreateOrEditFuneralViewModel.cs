using TDV.Burial.Dtos;
using System.Collections.Generic;
using Abp.Extensions;
using TDV.Location.Dtos;

namespace TDV.Web.Areas.Portal.Models.Funerals
{
    public class CreateOrEditFuneralViewModel
    {
        public CreateOrEditFuneralDto Funeral { get; set; }

        public string FuneralTypeDescription { get; set; }

        public string ContactDisplayProperty { get; set; }

        public string OwnerOrganizationUnitDisplayName { get; set; }

        public string GiverOrganizationUnitDisplayName { get; set; }

        public string ContractorOrganizationUnitDisplayName { get; set; }

        public string UserName { get; set; }

        public string FuneralPackageCode { get; set; }

        public string ContractFormule { get; set; }

        public string VehiclePlate { get; set; }

        public List<FuneralFuneralTypeLookupTableDto> FuneralFuneralTypeList { get; set; }

        public List<FuneralOrganizationUnitLookupTableDto> FuneralOrganizationUnitList { get; set; }
        public List<FuneralUserLookupTableDto> FuneralUserList { get; set; }

        public List<FuneralFuneralPackageLookupTableDto> FuneralFuneralPackageList { get; set; }

        public List<FuneralContractLookupTableDto> FuneralContractList { get; set; }

        public List<VehiclesLookupTableDto> FuneralVehicleList { get; set; }


        public int CountryId { get; set; }

        public int CityId { get; set; }

        public int DistrictId { get; set; }

        public int QuarterId { get; set; }

        public string RegionDisplayProperty { get; set; }

        public string AirlineDisplayProperty { get; set; }

        public string LiftOffAirportDisplayProperty { get; set; }

        public string LandingAirportDisplayProperty { get; set; }

        public bool IsEditMode => Funeral.Id.HasValue;
    }
}