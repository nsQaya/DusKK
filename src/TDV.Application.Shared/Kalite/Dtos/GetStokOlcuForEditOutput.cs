using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Kalite.Dtos
{
    public class GetStokOlcuForEditOutput
    {
        public CreateOrEditStokOlcuDto StokOlcu { get; set; }

        public string StokAdi { get; set; }

        public string OlcumOlcuTipi { get; set; }

    }
}