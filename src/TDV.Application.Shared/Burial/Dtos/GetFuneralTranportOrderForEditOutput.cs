using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Burial.Dtos
{
    public class GetFuneralTranportOrderForEditOutput
    {
        public CreateOrEditFuneralTranportOrderDto FuneralTranportOrder { get; set; }

        public string FuneralWorkOrderDetailDescription { get; set; }

    }
}