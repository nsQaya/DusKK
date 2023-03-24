using Abp.Application.Services.Dto;
using System;

namespace TDV.Burial.Dtos
{
    public class GetAllFuneralPackagesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? StatusFilter { get; set; }

        public string CodeFilter { get; set; }

        public string DescriptionFilter { get; set; }

    }
}