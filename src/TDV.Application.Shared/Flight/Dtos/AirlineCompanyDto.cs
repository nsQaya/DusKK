using System;
using Abp.Application.Services.Dto;

namespace TDV.Flight.Dtos
{
    public class AirlineCompanyDto : EntityDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string LadingPrefix { get; set; }

        public string FlightPrefix { get; set; }

    }
}