using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Communication.Dtos
{
    public class GetContactNetsisDetailForEditOutput
    {
        public CreateOrEditContactNetsisDetailDto ContactNetsisDetail { get; set; }

        public string ContactName { get; set; }

    }
}