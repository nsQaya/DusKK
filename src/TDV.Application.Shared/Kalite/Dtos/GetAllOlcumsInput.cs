﻿using Abp.Application.Services.Dto;
using System;

namespace TDV.Kalite.Dtos
{
    public class GetAllOlcumsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string OlcuTipiFilter { get; set; }

    }
}