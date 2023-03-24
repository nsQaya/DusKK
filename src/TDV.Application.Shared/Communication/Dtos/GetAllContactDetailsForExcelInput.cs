using Abp.Application.Services.Dto;
using System;

namespace TDV.Communication.Dtos
{
    public class GetAllContactDetailsForExcelInput
    {
        public string Filter { get; set; }

        public int? TypeFilter { get; set; }

        public string ValueFilter { get; set; }

        public string ContactIdentifyNoFilter { get; set; }

        public int? ContactIdFilter { get; set; }
    }
}