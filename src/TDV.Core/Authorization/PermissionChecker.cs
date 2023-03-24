using Abp.Authorization;
using TDV.Authorization.Roles;
using TDV.Authorization.Users;

namespace TDV.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
