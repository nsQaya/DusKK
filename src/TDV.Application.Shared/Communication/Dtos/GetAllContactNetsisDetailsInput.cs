using Abp.Application.Services.Dto;
using System;

namespace TDV.Communication.Dtos
{
    public class GetAllContactNetsisDetailsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NetsisNoFilter { get; set; }

        public string RegistryNoFilter { get; set; }

        public string ContactNameFilter { get; set; }

    }
}