using Abp.AutoMapper;
using TDV.Authorization.Roles.Dto;
using TDV.Web.Areas.Portal.Models.Common;

namespace TDV.Web.Areas.Portal.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode => Role.Id.HasValue;
    }
}