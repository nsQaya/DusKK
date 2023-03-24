using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using TDV.Authorization;

namespace TDV.Web.Areas.Portal.Startup
{
    public class PortalNavigationProvider : NavigationProvider
    {
        public const string MenuName = "App";

        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] = new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));

            menu
                .AddItem(new MenuItemDefinition(
                        PortalPageNames.Host.Dashboard,
                        L("Dashboard"),
                        url: "Portal/HostDashboard",
                        icon: "flaticon-line-graph",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Dashboard)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.Taleps,
                        L("Taleps"),
                        url: "Portal/Taleps",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Taleps)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.StokOlcus,
                        L("StokOlcus"),
                        url: "Portal/StokOlcus",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_StokOlcus)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.Stoks,
                        L("Stoks"),
                        url: "Portal/Stoks",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Stoks)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.Olcums,
                        L("Olcums"),
                        url: "Portal/Olcums",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Olcums)
                    )
                )

                /*.AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.CompanyContacts,
                        L("CompanyContacts"),
                        url: "Portal/CompanyContacts",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_CompanyContacts)
                    )
                )
                /*.AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.FuneralTranportOrders,
                        L("FuneralTranportOrders"),
                        url: "Portal/FuneralTranportOrders",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_FuneralTranportOrders)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.FuneralWorkOrderDetails,
                        L("FuneralWorkOrderDetails"),
                        url: "Portal/FuneralWorkOrderDetails",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_FuneralWorkOrderDetails)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.FuneralWorkOrders,
                        L("FuneralWorkOrders"),
                        url: "Portal/FuneralWorkOrders",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_FuneralWorkOrders)
                    )
                )*/

                .AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.Burial,
                        L("Burial"),
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Burial)
                    ).AddItem(new MenuItemDefinition(
                            PortalPageNames.Common.FuneralTypes,
                            L("FuneralTypes"),
                            url: "Portal/FuneralTypes",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_FuneralTypes)
                        )
                    ).AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.FuneralFlights,
                        L("FuneralFlights"),
                        url: "Portal/FuneralFlights",
                        icon: "flaticon-more",
                                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_FuneralFlights)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                    PortalPageNames.Common.Funerals,
                                    L("Funerals"),
                                    url: "Portal/Funerals",
                                    icon: "flaticon-more",
                                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Funerals)
                                )
                            ).AddItem(new MenuItemDefinition(
                                PortalPageNames.Common.FuneralDocuments,
                                L("FuneralDocuments"),
                                url: "Portal/FuneralDocuments",
                                icon: "flaticon-more",
                                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_FuneralDocuments)
                            )
                        ).AddItem(new MenuItemDefinition(
                            PortalPageNames.Common.FuneralAddreses,
                            L("FuneralAddreses"),
                            url: "Portal/FuneralAddreses",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_FuneralAddreses)
                        )
                    ).AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.FuneralPackages,
                        L("FuneralPackages"),
                        url: "Portal/FuneralPackages",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_FuneralPackages)
                    )
                ).AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.FuneralDriver,
                        L("FuneralDriver"),
                        url: "Portal/Funerals/Driver",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Funerals_Driver)
                    )
                ).AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.FuneralAssignment,
                        L("FuneralAssignment"),
                        url: "Portal/Funerals/Assignment",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Funerals_Funeral_Assignment)
                    )
                ).AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.FuneralDriverAssignment,
                        L("FuneralDriverAssignment"),
                        url: "Portal/Funerals/DriverAssignment",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Funerals_Driver_Assignment)
                    )
                )
                )
                .AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.Communication,
                        L("Communication"),
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Communication)
                    ).AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.Contacts,
                        L("Contacts"),
                        url: "Portal/Contacts",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Contacts)
                        )
                    )
                )
                .AddItem(new MenuItemDefinition(
                    PortalPageNames.Common.Fixed,
                    L("Fixed"),
                    icon: "flaticon-book",
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Fixed)
                    )
                .AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.Currencies,
                        L("Currencies"),
                        url: "Portal/Currencies",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Currencies)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.DataLists,
                        L("DataLists"),
                        url: "Portal/DataLists",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_DataLists)
                    )
                ).AddItem(new MenuItemDefinition(
                            PortalPageNames.Common.Corporation,
                            L("Corporation"),
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Corporation)
                        ).AddItem(new MenuItemDefinition(
                                PortalPageNames.Common.Companies,
                                L("Companies"),
                                url: "Portal/Companies",
                                icon: "flaticon-more",
                                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Companies)
                            )
                        ).AddItem(new MenuItemDefinition(
                                PortalPageNames.Common.Vehicles,
                                L("Vehicles"),
                                url: "Portal/Vehicles",
                                icon: "flaticon-more",
                                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Vehicles)
                            )
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.Location,
                        L("Location"),
                        icon: "flaticon-map",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Location)
                        )
                        .AddItem(new MenuItemDefinition(
                            PortalPageNames.Common.Countries,
                            L("Countries"),
                            url: "Portal/Countries",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Countries)
                                )
                            )
                        .AddItem(new MenuItemDefinition(
                                PortalPageNames.Common.Cities,
                                L("Cities"),
                                url: "Portal/Cities",
                                icon: "flaticon-more",
                                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Cities)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PortalPageNames.Common.Districts,
                                L("Districts"),
                                url: "Portal/Districts",
                                icon: "flaticon-more",
                                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Districts)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                PortalPageNames.Common.Quarters,
                                L("Quarters"),
                                url: "Portal/Quarters",
                                icon: "flaticon-more",
                                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Quarters)
                            )
                        )
                    ).AddItem(new MenuItemDefinition(
                            PortalPageNames.Common.Regions,
                            L("Regions"),
                            url: "Portal/Regions",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Regions)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.Airports,
                        L("Flight"),
                        icon: "flaticon-paper-plane",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Flight)
                        ).AddItem(new MenuItemDefinition(
                            PortalPageNames.Common.Airports,
                            L("Airports"),
                            url: "Portal/Airports",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Airports)
                            )
                        ).AddItem(new MenuItemDefinition(
                                PortalPageNames.Common.AirlineCompanies,
                                L("AirlineCompanies"),
                                url: "Portal/AirlineCompanies",
                                icon: "flaticon-more",
                                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_AirlineCompanies)
                            )
                        )
                    ).AddItem(new MenuItemDefinition(
                            PortalPageNames.Common.Payment,
                            L("Payment"),
                            icon: "flaticon2-analytics-2",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Payment)
                        ).AddItem(new MenuItemDefinition(
                                PortalPageNames.Common.Contracts,
                                L("Contracts"),
                                url: "Portal/Contracts",
                                icon: "flaticon-more",
                                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Contracts)
                            )
                        ).AddItem(new MenuItemDefinition(
                                PortalPageNames.Common.ContractFormules,
                                L("ContractFormules"),
                                url: "Portal/ContractFormules",
                                icon: "flaticon-more",
                                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_ContractFormules)
                            )
                        ).AddItem(new MenuItemDefinition(
                                PortalPageNames.Common.FixedPrices,
                                L("FixedPrices"),
                                url: "Portal/FixedPrices",
                                icon: "flaticon-more",
                                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_FixedPrices)
                            )
                        ).AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.CompanyTransactions,
                        L("CompanyTransactions"),
                        url: "Portal/CompanyTransactions",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_CompanyTransactions)
                    )
                )
                    )
                )
                .AddItem(new MenuItemDefinition(
                        PortalPageNames.Host.Tenants,
                        L("Tenants"),
                        url: "Portal/Tenants",
                        icon: "flaticon-list-3",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenants)
                    )
                ).AddItem(new MenuItemDefinition(
                        PortalPageNames.Host.Editions,
                        L("Editions"),
                        url: "Portal/Editions",
                        icon: "flaticon-app",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Editions)
                    )
                ).AddItem(new MenuItemDefinition(
                        PortalPageNames.Tenant.Dashboard,
                        L("Dashboard"),
                        url: "Portal/TenantDashboard",
                        icon: "flaticon-line-graph",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenant_Dashboard)
                    )
                ).AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.Administration,
                        L("Administration"),
                        icon: "flaticon-interface-8"
                    ).AddItem(new MenuItemDefinition(
                            PortalPageNames.Common.OrganizationUnits,
                            L("OrganizationUnits"),
                            url: "Portal/OrganizationUnits",
                            icon: "flaticon-map",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_OrganizationUnits)
                        )
                    ).AddItem(new MenuItemDefinition(
                            PortalPageNames.Common.Roles,
                            L("Roles"),
                            url: "Portal/Roles",
                            icon: "flaticon-suitcase",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Roles)
                        )
                    ).AddItem(new MenuItemDefinition(
                            PortalPageNames.Common.Users,
                            L("Users"),
                            url: "Portal/Users",
                            icon: "flaticon-users",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Users)
                        )
                    ).AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.UserDetails,
                        L("UserDetails"),
                        url: "Portal/UserDetails",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_UserDetails)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PortalPageNames.Common.Languages,
                            L("Languages"),
                            url: "Portal/Languages",
                            icon: "flaticon-tabs",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Languages)
                        )
                    ).AddItem(new MenuItemDefinition(
                            PortalPageNames.Common.AuditLogs,
                            L("AuditLogs"),
                            url: "Portal/AuditLogs",
                            icon: "flaticon-folder-1",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_AuditLogs)
                        )
                    ).AddItem(new MenuItemDefinition(
                            PortalPageNames.Host.Maintenance,
                            L("Maintenance"),
                            url: "Portal/Maintenance",
                            icon: "flaticon-lock",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Host_Maintenance)
                        )
                    ).AddItem(new MenuItemDefinition(
                            PortalPageNames.Tenant.SubscriptionManagement,
                            L("Subscription"),
                            url: "Portal/SubscriptionManagement",
                            icon: "flaticon-refresh",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Tenant_SubscriptionManagement)
                        )
                    ).AddItem(new MenuItemDefinition(
                            PortalPageNames.Common.UiCustomization,
                            L("VisualSettings"),
                            url: "Portal/UiCustomization",
                            icon: "flaticon-medical",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_UiCustomization)
                        )
                    ).AddItem(new MenuItemDefinition(
                            PortalPageNames.Common.WebhookSubscriptions,
                            L("WebhookSubscriptions"),
                            url: "Portal/WebhookSubscription",
                            icon: "flaticon2-world",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_WebhookSubscription)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PortalPageNames.Common.DynamicProperties,
                            L("DynamicProperties"),
                            url: "Portal/DynamicProperty",
                            icon: "flaticon-interface-8",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_DynamicProperties)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PortalPageNames.Host.Settings,
                            L("Settings"),
                            url: "Portal/HostSettings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Host_Settings)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            PortalPageNames.Tenant.Settings,
                            L("Settings"),
                            url: "Portal/Settings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Tenant_Settings)
                        )
                    ).AddItem(new MenuItemDefinition(
                            PortalPageNames.Common.Notifications,
                            L("Notifications"),
                            icon: "flaticon-alarm"
                        ).AddItem(new MenuItemDefinition(
                                PortalPageNames.Common.Notifications_Inbox,
                                L("Inbox"),
                                url: "Portal/Notifications",
                                icon: "flaticon-mail-1"
                            )
                        ).AddItem(new MenuItemDefinition(
                                PortalPageNames.Common.Notifications_MassNotifications,
                                L("MassNotifications"),
                                url: "Portal/Notifications/MassNotifications",
                                icon: "flaticon-paper-plane",
                                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_MassNotification)
                            )
                        )
                    )
                ).AddItem(new MenuItemDefinition(
                        PortalPageNames.Common.DemoUiComponents,
                        L("DemoUiComponents"),
                        url: "Portal/DemoUiComponents",
                        icon: "flaticon-shapes",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_DemoUiComponents)
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TDVConsts.LocalizationSourceName);
        }
    }
}