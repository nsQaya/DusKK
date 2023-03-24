using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Location.Dtos
{
    public class GetCityForEditOutput
    {
        public CreateOrEditCityDto City { get; set; }

        public string CountryDisplayProperty { get; set; }

    }
}