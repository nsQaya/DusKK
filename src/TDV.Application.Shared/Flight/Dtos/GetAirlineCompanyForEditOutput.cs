using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Flight.Dtos
{
    public class GetAirlineCompanyForEditOutput
    {
        public CreateOrEditAirlineCompanyDto AirlineCompany { get; set; }

    }
}