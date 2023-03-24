﻿using Abp.Application.Services.Dto;
using System;

namespace TDV.Location.Dtos
{
    public class GetAllRegionsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public int? MaxOrderFilter { get; set; }
        public int? MinOrderFilter { get; set; }

        public int? IsActiveFilter { get; set; }

        public string FixedPriceNameFilter { get; set; }

    }
}