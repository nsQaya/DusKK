using TDV.Burial;

using System;
using Abp.Application.Services.Dto;

namespace TDV.Burial.Dtos
{
    public class FuneralDocumentDto : EntityDto
    {
        public FuneralDocumentType Type { get; set; }

        public string Path { get; set; }

        public Guid Guid { get; set; }

        public int FuneralId { get; set; }

    }
}