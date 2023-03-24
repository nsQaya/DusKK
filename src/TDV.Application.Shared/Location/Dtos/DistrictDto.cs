using System;
using Abp.Application.Services.Dto;

namespace TDV.Location.Dtos
{
    public class DistrictDto : EntityDto
    {
        public string Name { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }

        public int CityId { get; set; }

        public int RegionId { get; set; }

    }
}