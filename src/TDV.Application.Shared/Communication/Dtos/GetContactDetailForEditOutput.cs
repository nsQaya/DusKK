using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Communication.Dtos
{
    public class GetContactDetailForEditOutput
    {
        public CreateOrEditContactDetailDto ContactDetail { get; set; }

        public string ContactIdentifyNo { get; set; }

    }
}