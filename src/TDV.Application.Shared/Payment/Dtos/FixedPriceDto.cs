using System;
using Abp.Application.Services.Dto;

namespace TDV.Payment.Dtos
{
    public class FixedPriceDto : EntityDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

    }
}