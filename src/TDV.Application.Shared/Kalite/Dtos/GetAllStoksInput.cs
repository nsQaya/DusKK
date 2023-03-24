using Abp.Application.Services.Dto;
using System;

namespace TDV.Kalite.Dtos
{
    public class GetAllStoksInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string KoduFilter { get; set; }

        public string AdiFilter { get; set; }

    }
}