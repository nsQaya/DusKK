using System;
using Abp.Application.Services.Dto;

namespace TDV.Corporation.Dtos
{
    public class CompanyContactDto : EntityDto
    {
        public string Title { get; set; }

        public int CompanyId { get; set; }

        public int? ContactId { get; set; }

    }
}