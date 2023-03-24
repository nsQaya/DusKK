using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Burial.Dtos
{
    public class GetFuneralPackageForEditOutput
    {
        public CreateOrEditFuneralPackageDto FuneralPackage { get; set; }

    }
}