namespace TDV.Burial.Dtos
{
    public class GetFuneralForViewDto
    {
        public FuneralDto Funeral { get; set; }

        public string FuneralTypeDescription { get; set; }

        public string ContactDisplayProperty { get; set; }

        public string OwnerOrganizationUnitDisplayName { get; set; }

        public string GiverOrganizationUnitDisplayName { get; set; }

        public string ContractorOrganizationUnitDisplayName { get; set; }

        public string UserName { get; set; }

        public string FuneralPackageCode { get; set; }

        public string ContractFormule { get; set; }

        public string VehiclePlate { get; set; }

        public string CountryDisplayName { get; set; }

        public string CityDisplayName { get; set; }

        public string DistrictDisplayName { get; set; }

        public string QuarterDisplayName { get; set; }

        public string RegionDisplayName { get; set; }

        public string LiftOffAirportDisplayName { get; set; }

        public string LandingAirportDisplayName { get; set; }

        public string AirlineCompanyDisplayName { get; set; }

    }
}