using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Location.Dtos
{
    public class GetRegionForEditOutput
    {
        public CreateOrEditRegionDto Region { get; set; }

        public string FixedPriceName { get; set; }

    }
}