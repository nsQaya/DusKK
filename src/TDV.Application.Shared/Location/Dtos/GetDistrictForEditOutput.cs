using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Location.Dtos
{
    public class GetDistrictForEditOutput
    {
        public CreateOrEditDistrictDto District { get; set; }

        public string CityDisplayProperty { get; set; }

        public string RegionName { get; set; }

    }
}