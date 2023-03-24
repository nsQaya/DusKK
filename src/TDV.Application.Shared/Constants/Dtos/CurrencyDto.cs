using System;
using Abp.Application.Services.Dto;

namespace TDV.Constants.Dtos
{
    public class CurrencyDto : EntityDto
    {
        public string Code { get; set; }

        public string Symbol { get; set; }

        public int OrderNumber { get; set; }

        public int NumberCode { get; set; }

        public bool IsActive { get; set; }

    }
}