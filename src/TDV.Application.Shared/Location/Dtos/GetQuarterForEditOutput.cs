using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Location.Dtos
{
    public class GetQuarterForEditOutput
    {
        public CreateOrEditQuarterDto Quarter { get; set; }

        public string DistrictName { get; set; }

    }
}