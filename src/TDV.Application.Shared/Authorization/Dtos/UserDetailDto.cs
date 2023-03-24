using System;
using Abp.Application.Services.Dto;

namespace TDV.Authorization.Dtos
{
    public class UserDetailDto : EntityDto
    {

        public long? UserId { get; set; }

        public int? ContactId { get; set; }

    }
}