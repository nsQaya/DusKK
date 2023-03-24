using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Payment.Dtos
{
    public class GetContractForEditOutput
    {
        public CreateOrEditContractDto Contract { get; set; }

        public string RegionName { get; set; }

        public string CompanyDisplayProperty { get; set; }

    }
}