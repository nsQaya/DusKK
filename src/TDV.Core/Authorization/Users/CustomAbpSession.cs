using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TDV.Authorization.Users
{
    public class CustomAbpSession : ClaimsAbpSession, ITransientDependency
    {
        
        public CustomAbpSession(
            IPrincipalAccessor principalAccessor,
            IMultiTenancyConfig multiTenancy,
            ITenantResolver tenantResolver,
            IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider) :
            base(principalAccessor, multiTenancy, tenantResolver, sessionOverrideScopeProvider)
        {
        }


        public List<long> OrganizationUnits
        {
            get
            {
                var cliamUnits = PrincipalAccessor.Principal?.Claims.First(x => x.Type == "User.OrgUnits")?.Value;
                return cliamUnits.Split(',').Select(long.Parse).ToList();
            }
        }
        public List<string> Roles
        {
            get
            {
                var cliamRoles = PrincipalAccessor.Principal?.Claims.First(x => x.Type == "User.Roles")?.Value;
                return cliamRoles.Split(',').ToList();
            }
        }
    }
}
