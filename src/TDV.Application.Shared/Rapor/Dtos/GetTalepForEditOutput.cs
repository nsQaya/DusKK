using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Rapor.Dtos
{
    public class GetTalepForEditOutput
    {
        public CreateOrEditTalepDto Talep { get; set; }

        public string StokAdi { get; set; }

    }
}