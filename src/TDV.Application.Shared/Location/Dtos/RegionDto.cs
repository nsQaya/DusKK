using System;
using Abp.Application.Services.Dto;

namespace TDV.Location.Dtos
{
    public class RegionDto : EntityDto
    {
        public string Name { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }

        public int FixedPriceId { get; set; }

    }
}