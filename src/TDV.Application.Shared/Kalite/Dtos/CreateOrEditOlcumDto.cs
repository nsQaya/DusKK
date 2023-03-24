using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Kalite.Dtos
{
    public class CreateOrEditOlcumDto : EntityDto<int?>
    {

        [Required]
        public string OlcuTipi { get; set; }

    }
}