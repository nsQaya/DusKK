using System.Linq;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TDV.Authorization;
using TDV.Authorization.Roles;
using TDV.Authorization.Users;
using TDV.EntityFrameworkCore;
using TDV.Notifications;

namespace TDV.Migrations.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly TDVDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(TDVDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            //Admin role

            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            //User role

            var userRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.User);
            if (userRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.User, StaticRoleNames.Tenants.User) { IsStatic = true, IsDefault = true });
                _context.SaveChanges();
            }

            #region Cenaze için sabit roller
            var entererRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Enterer);
            if (entererRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Enterer, StaticRoleNames.Tenants.Enterer) { IsStatic = true });
                _context.SaveChanges();
            }

            var employeeRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Employee);
            if (employeeRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Employee, StaticRoleNames.Tenants.Employee) { IsStatic = true });
                _context.SaveChanges();
            }

            var driverRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Driver);
            if (driverRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Driver, StaticRoleNames.Tenants.Driver) { IsStatic = true });
                _context.SaveChanges();
            }

            var managerRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Manager);
            if (managerRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Manager, StaticRoleNames.Tenants.Manager) { IsStatic = true });
                _context.SaveChanges();
            }
            #endregion

            //admin user

            var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "admin@defaulttenant.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.ShouldChangePasswordOnNextLogin = false;
                adminUser.IsActive = true;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                //Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();

                //User account of admin user
                if (_tenantId == 1)
                {
                    _context.UserAccounts.Add(new UserAccount
                    {
                        TenantId = _tenantId,
                        UserId = adminUser.Id,
                        UserName = AbpUserBase.AdminUserName,
                        EmailAddress = adminUser.EmailAddress
                    });
                    _context.SaveChanges();
                }

                //Notification subscription
                _context.NotificationSubscriptions.Add(new NotificationSubscriptionInfo(SequentialGuidGenerator.Instance.Create(), _tenantId, adminUser.Id, AppNotificationNames.NewUserRegistered));
                _context.SaveChanges();
            }
        }
    }
}
