using TDV.Burial;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Burial.Dtos
{
    public class CreateOrEditFuneralDocumentDto : EntityDto<int?>
    {

        public FuneralDocumentType Type { get; set; }

        [Required]
        public string Path { get; set; }

        public Guid Guid { get; set; }

        public int FuneralId { get; set; }

    }
}