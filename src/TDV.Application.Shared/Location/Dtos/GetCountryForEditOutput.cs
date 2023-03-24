using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Location.Dtos
{
    public class GetCountryForEditOutput
    {
        public CreateOrEditCountryDto Country { get; set; }

    }
}