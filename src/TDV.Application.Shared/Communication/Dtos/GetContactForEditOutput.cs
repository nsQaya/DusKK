using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Communication.Dtos
{
    public class GetContactForEditOutput
    {
        public CreateOrEditContactDto Contact { get; set; }

    }
}