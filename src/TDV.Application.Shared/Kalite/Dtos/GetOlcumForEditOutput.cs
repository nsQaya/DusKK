using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Kalite.Dtos
{
    public class GetOlcumForEditOutput
    {
        public CreateOrEditOlcumDto Olcum { get; set; }

    }
}