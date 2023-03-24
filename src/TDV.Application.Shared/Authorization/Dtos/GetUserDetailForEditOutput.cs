using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Authorization.Dtos
{
    public class GetUserDetailForEditOutput
    {
        public CreateOrEditUserDetailDto UserDetail { get; set; }

        public string UserName { get; set; }

        public string ContactDisplayProperty { get; set; }

    }
}