using System.Linq;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using TDV.Authorization.Users.Dto;
using TDV.Security;
using TDV.Web.Areas.Portal.Models.Common;

namespace TDV.Web.Areas.Portal.Models.Users
{
    [AutoMapFrom(typeof(GetUserForEditOutput))]
    public class CreateOrEditUserModalViewModel : GetUserForEditOutput, IOrganizationUnitsEditViewModel
    {
        public bool CanChangeUserName => User.UserName != AbpUserBase.AdminUserName;

        public int AssignedRoleCount
        {
            get { return Roles.Count(r => r.IsAssigned); }
        }

        public bool IsEditMode => User.Id.HasValue;

        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public string AllowedUserNameCharacters { get; set; }
    }
}