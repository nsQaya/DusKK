using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using TDV.Authorization.Users;
using TDV.MultiTenancy;

namespace TDV.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}