using Abp.Application.Services.Dto;
using System;

namespace TDV.Authorization.Dtos
{
    public class GetAllUserDetailsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string UserNameFilter { get; set; }

        public string ContactDisplayPropertyFilter { get; set; }

    }
}