using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Kalite.Dtos
{
    public class CreateOrEditStokDto : EntityDto<int?>
    {

        [Required]
        public string Kodu { get; set; }

        [Required]
        public string Adi { get; set; }

    }
}