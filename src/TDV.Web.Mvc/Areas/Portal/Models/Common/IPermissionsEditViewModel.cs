using System.Collections.Generic;
using TDV.Authorization.Permissions.Dto;

namespace TDV.Web.Areas.Portal.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}