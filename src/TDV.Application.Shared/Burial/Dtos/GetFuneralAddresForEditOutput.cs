using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using TDV.Location.Dtos;

namespace TDV.Burial.Dtos
{
    public class GetFuneralAddresForEditOutput
    {
        public CreateOrEditFuneralAddresDto FuneralAddres { get; set; }

        public string FuneralDisplayProperty { get; set; }

        public string RegionName { get; set; }
        public string QuarterName { get; set; }

        public List<CountryLookupTableDto> CountryList { get; set; }
        public int CountryId { get; set; }
        public List<CityLookupTableDto> CityList { get; set; }
        public int CityId { get; set; }
        public List<DistrictLookupTableDto> DistrictList { get; set; }
        public int DistrictId { get; set; }
        public List<QuarterLookupTableDto> QuarterList { get; set; }

    }
}