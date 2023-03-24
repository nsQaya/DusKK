﻿using Abp.Application.Services.Dto;
using System;

namespace TDV.Communication.Dtos
{
    public class GetAllContactsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string SurnameFilter { get; set; }

        public string IdentifyNoFilter { get; set; }

    }
}