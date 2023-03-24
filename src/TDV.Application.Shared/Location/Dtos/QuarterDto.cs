using System;
using Abp.Application.Services.Dto;

namespace TDV.Location.Dtos
{
    public class QuarterDto : EntityDto
    {
        public string Name { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }

        public int DistrictId { get; set; }

    }
}