using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Burial.Dtos
{
    public class GetFuneralDocumentForEditOutput
    {
        public CreateOrEditFuneralDocumentDto FuneralDocument { get; set; }

        public string FuneralDisplayProperty { get; set; }
        public string PathFileName { get; set; }

    }
}