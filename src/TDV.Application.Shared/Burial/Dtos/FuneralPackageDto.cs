using TDV.Burial;

using System;
using Abp.Application.Services.Dto;

namespace TDV.Burial.Dtos
{
    public class FuneralPackageDto : EntityDto
    {
        public FuneralStatus Status { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

    }
}