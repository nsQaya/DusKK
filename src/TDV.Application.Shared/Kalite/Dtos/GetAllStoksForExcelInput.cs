using Abp.Application.Services.Dto;
using System;

namespace TDV.Kalite.Dtos
{
    public class GetAllStoksForExcelInput
    {
        public string Filter { get; set; }

        public string KoduFilter { get; set; }

        public string AdiFilter { get; set; }

    }
}