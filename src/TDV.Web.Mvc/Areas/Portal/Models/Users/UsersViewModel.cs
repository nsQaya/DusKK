using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TDV.Authorization.Permissions.Dto;
using TDV.Web.Areas.Portal.Models.Common;

namespace TDV.Web.Areas.Portal.Models.Users
{
    public class UsersViewModel : IPermissionsEditViewModel
    {
        public string FilterText { get; set; }

        public List<ComboboxItemDto> Roles { get; set; }

        public bool OnlyLockedUsers { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}
