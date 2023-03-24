using Abp.Authorization;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TDV.Authorization.Roles;

namespace TDV.Authorization.Users
{
    public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory<User, Role>
    {
        private readonly UserManager _userManager;
        public UserClaimsPrincipalFactory(
            UserManager userManager,
            RoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            IUnitOfWorkManager unitOfWorkManager)
            : base(
                  userManager,
                  roleManager,
                  optionsAccessor,
                  unitOfWorkManager)
        {
            _userManager= userManager;
        }
        public async override Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var claim = await base.CreateAsync(user);
            
            var units = await _userManager.GetOrganizationUnitsAsync(user); 
            var roles = await _userManager.GetRolesAsync(user);


            claim.Identities.First().AddClaim(new Claim("User.OrgUnits", units.Count() > 0 ? ( string.Join(',', units?.Select(x => x.Id))) : "0"));
            claim.Identities.First().AddClaim(new Claim("User.Roles", roles.Count() > 0 ? ( string.Join(',', roles?.Select(x => x))) : "0"));

            return claim;
        }
    }
}
