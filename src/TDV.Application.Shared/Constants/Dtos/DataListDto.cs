using System;
using Abp.Application.Services.Dto;

namespace TDV.Constants.Dtos
{
    public class DataListDto : EntityDto
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public int OrderNumber { get; set; }

        public bool IsActive { get; set; }

    }
}