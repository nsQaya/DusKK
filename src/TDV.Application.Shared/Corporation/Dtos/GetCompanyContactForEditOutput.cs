using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Corporation.Dtos
{
    public class GetCompanyContactForEditOutput
    {
        public CreateOrEditCompanyContactDto CompanyContact { get; set; }

        public string CompanyDisplayProperty { get; set; }

        public string ContactName { get; set; }

    }
}