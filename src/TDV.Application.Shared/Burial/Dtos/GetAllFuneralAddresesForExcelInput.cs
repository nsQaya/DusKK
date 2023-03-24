using Abp.Application.Services.Dto;
using System;

namespace TDV.Burial.Dtos
{
    public class GetAllFuneralAddresesForExcelInput
    {
        public string Filter { get; set; }

        public string DescriptionFilter { get; set; }

        public string AddressFilter { get; set; }

        public string FuneralDisplayPropertyFilter { get; set; }

        public string QuarterNameFilter { get; set; }

    }
}