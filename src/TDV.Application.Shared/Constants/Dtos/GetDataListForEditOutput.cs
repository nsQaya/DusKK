using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Constants.Dtos
{
    public class GetDataListForEditOutput
    {
        public CreateOrEditDataListDto DataList { get; set; }

    }
}