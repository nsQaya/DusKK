﻿using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Burial.Dtos
{
    public class GetFuneralForEditOutput
    {
        public CreateOrEditFuneralDto Funeral { get; set; }

        public string FuneralTypeDescription { get; set; }

        public string ContactDisplayProperty { get; set; }

        public string OwnerOrganizationUnitDisplayName { get; set; }

        public string GiverOrganizationUnitDisplayName { get; set; }

        public string ContractorOrganizationUnitDisplayName { get; set; }

        public string UserName { get; set; }

        public string FuneralPackageCode { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int QuarterId { get; set; }
        public string RegionDisplayProperty { get; set; }

        public string AirlineDisplayProperty { get; set; }
        public string LiftOffAirportDisplayProperty { get; set; }
        public string LandingAirportDisplayProperty { get; set; }

        public string ContractFormule { get; set; }

        public string VehiclePlate { get; set; }

    }
}