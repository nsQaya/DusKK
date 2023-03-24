using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Flight.Dtos
{
    public class GetAirportForEditOutput
    {
        public CreateOrEditAirportDto Airport { get; set; }

        public string CountryDisplayProperty { get; set; }

        public string CityDisplayProperty { get; set; }

    }
}