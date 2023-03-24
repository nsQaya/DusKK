using Abp.Application.Services.Dto;
using System;

namespace TDV.Burial.Dtos
{
    public class GetAllFuneralTypesForExcelInput
    {
        public string Filter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? IsDefaultFilter { get; set; }

    }
}