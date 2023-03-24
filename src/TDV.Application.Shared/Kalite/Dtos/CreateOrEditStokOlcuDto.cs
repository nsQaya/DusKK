using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Kalite.Dtos
{
    public class CreateOrEditStokOlcuDto : EntityDto<int?>
    {

        public decimal Alt { get; set; }

        public decimal Ust { get; set; }

        public string Deger { get; set; }

        public int StokId { get; set; }

        public int OlcumId { get; set; }

    }
}