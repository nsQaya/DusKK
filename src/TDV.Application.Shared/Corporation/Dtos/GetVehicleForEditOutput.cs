using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Corporation.Dtos
{
    public class GetVehicleForEditOutput
    {
        public CreateOrEditVehicleDto Vehicle { get; set; }

        public string CompanyDisplayProperty { get; set; }

    }
}