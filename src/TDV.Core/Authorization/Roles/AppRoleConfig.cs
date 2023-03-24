using Abp.MultiTenancy;
using Abp.Zero.Configuration;

namespace TDV.Authorization.Roles
{
    public static class AppRoleConfig
    {
        public static void Configure(IRoleManagementConfig roleManagementConfig)
        {
            //Static host roles

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Host.Admin,
                    MultiTenancySides.Host,
                    grantAllPermissionsByDefault: true)
                );

            //Static tenant roles

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Tenants.Admin,
                    MultiTenancySides.Tenant,
                    grantAllPermissionsByDefault: true)
                );

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Tenants.User,
                    MultiTenancySides.Tenant)
                );

            roleManagementConfig.StaticRoles.Add(
               new StaticRoleDefinition(
                   StaticRoleNames.Tenants.Enterer,
                   MultiTenancySides.Tenant)
               );

            roleManagementConfig.StaticRoles.Add(
               new StaticRoleDefinition(
                   StaticRoleNames.Tenants.Employee,
                   MultiTenancySides.Tenant)
               );
            roleManagementConfig.StaticRoles.Add(
               new StaticRoleDefinition(
                   StaticRoleNames.Tenants.Driver,
                   MultiTenancySides.Tenant)
               );
        }
    }
}
