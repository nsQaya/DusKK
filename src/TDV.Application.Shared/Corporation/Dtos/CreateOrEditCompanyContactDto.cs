using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Corporation.Dtos
{
    public class CreateOrEditCompanyContactDto : EntityDto<int?>
    {

        [Required]
        [StringLength(CompanyContactConsts.MaxTitleLength, MinimumLength = CompanyContactConsts.MinTitleLength)]
        public string Title { get; set; }

        public int CompanyId { get; set; }

        public int? ContactId { get; set; }

    }
}