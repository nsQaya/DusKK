using System;
using Abp.Application.Services.Dto;

namespace TDV.Location.Dtos
{
    public class CountryDto : EntityDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }

    }
}