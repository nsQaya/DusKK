using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Authorization.Dtos
{
    public class CreateOrEditUserDetailDto : EntityDto<int?>
    {

        public long? UserId { get; set; }

        public int? ContactId { get; set; }

    }
}