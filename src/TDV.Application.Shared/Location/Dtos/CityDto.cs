using System;
using Abp.Application.Services.Dto;

namespace TDV.Location.Dtos
{
    public class CityDto : EntityDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }

        public int CountryId { get; set; }

    }
}