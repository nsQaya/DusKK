﻿using Abp.Application.Services.Dto;

namespace TDV.Corporation.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}