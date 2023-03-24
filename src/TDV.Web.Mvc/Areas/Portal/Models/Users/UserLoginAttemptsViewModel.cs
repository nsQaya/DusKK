using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace TDV.Web.Areas.Portal.Models.Users
{
    public class UserLoginAttemptsViewModel
    {
        public List<ComboboxItemDto> LoginAttemptResults { get; set; }
    }
}