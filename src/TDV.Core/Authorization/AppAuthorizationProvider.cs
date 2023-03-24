using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace TDV.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var taleps = pages.CreateChildPermission(AppPermissions.Pages_Taleps, L("Taleps"));
            taleps.CreateChildPermission(AppPermissions.Pages_Taleps_Create, L("CreateNewTalep"));
            taleps.CreateChildPermission(AppPermissions.Pages_Taleps_Edit, L("EditTalep"));
            taleps.CreateChildPermission(AppPermissions.Pages_Taleps_Delete, L("DeleteTalep"));

            var stokOlcus = pages.CreateChildPermission(AppPermissions.Pages_StokOlcus, L("StokOlcus"));
            stokOlcus.CreateChildPermission(AppPermissions.Pages_StokOlcus_Create, L("CreateNewStokOlcu"));
            stokOlcus.CreateChildPermission(AppPermissions.Pages_StokOlcus_Edit, L("EditStokOlcu"));
            stokOlcus.CreateChildPermission(AppPermissions.Pages_StokOlcus_Delete, L("DeleteStokOlcu"));

            var stoks = pages.CreateChildPermission(AppPermissions.Pages_Stoks, L("Stoks"));
            stoks.CreateChildPermission(AppPermissions.Pages_Stoks_Create, L("CreateNewStok"));
            stoks.CreateChildPermission(AppPermissions.Pages_Stoks_Edit, L("EditStok"));
            stoks.CreateChildPermission(AppPermissions.Pages_Stoks_Delete, L("DeleteStok"));

            var olcums = pages.CreateChildPermission(AppPermissions.Pages_Olcums, L("Olcums"));
            olcums.CreateChildPermission(AppPermissions.Pages_Olcums_Create, L("CreateNewOlcum"));
            olcums.CreateChildPermission(AppPermissions.Pages_Olcums_Edit, L("EditOlcum"));
            olcums.CreateChildPermission(AppPermissions.Pages_Olcums_Delete, L("DeleteOlcum"));

            var fixedVariables = pages.CreateChildPermission(AppPermissions.Pages_Fixed, L("Fixed"));
            #region constants

            var currencies = fixedVariables.CreateChildPermission(AppPermissions.Pages_Currencies, L("Currencies"));
            currencies.CreateChildPermission(AppPermissions.Pages_Currencies_Create, L("CreateNewCurrency"));
            currencies.CreateChildPermission(AppPermissions.Pages_Currencies_Edit, L("EditCurrency"));
            currencies.CreateChildPermission(AppPermissions.Pages_Currencies_Delete, L("DeleteCurrency"));

            var dataLists = fixedVariables.CreateChildPermission(AppPermissions.Pages_DataLists, L("DataLists"));
            dataLists.CreateChildPermission(AppPermissions.Pages_DataLists_Create, L("CreateNewDataList"));
            dataLists.CreateChildPermission(AppPermissions.Pages_DataLists_Edit, L("EditDataList"));
            dataLists.CreateChildPermission(AppPermissions.Pages_DataLists_Delete, L("DeleteDataList"));
            #endregion
            var location = fixedVariables.CreateChildPermission(AppPermissions.Pages_Location, L("Location"));

            #region location
            var countries = location.CreateChildPermission(AppPermissions.Pages_Countries, L("Countries"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_Create, L("CreateNewCountry"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_Edit, L("EditCountry"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_Delete, L("DeleteCountry"));

            var cities = location.CreateChildPermission(AppPermissions.Pages_Cities, L("Cities"));
            cities.CreateChildPermission(AppPermissions.Pages_Cities_Create, L("CreateNewCity"));
            cities.CreateChildPermission(AppPermissions.Pages_Cities_Edit, L("EditCity"));
            cities.CreateChildPermission(AppPermissions.Pages_Cities_Delete, L("DeleteCity"));

            var districts = location.CreateChildPermission(AppPermissions.Pages_Districts, L("Districts"));
            districts.CreateChildPermission(AppPermissions.Pages_Districts_Create, L("CreateNewDistrict"));
            districts.CreateChildPermission(AppPermissions.Pages_Districts_Edit, L("EditDistrict"));
            districts.CreateChildPermission(AppPermissions.Pages_Districts_Delete, L("DeleteDistrict"));

            var quarters = location.CreateChildPermission(AppPermissions.Pages_Quarters, L("Quarters"));
            quarters.CreateChildPermission(AppPermissions.Pages_Quarters_Create, L("CreateNewQuarter"));
            quarters.CreateChildPermission(AppPermissions.Pages_Quarters_Edit, L("EditQuarter"));
            quarters.CreateChildPermission(AppPermissions.Pages_Quarters_Delete, L("DeleteQuarter"));
            #endregion

            var regions = fixedVariables.CreateChildPermission(AppPermissions.Pages_Regions, L("Regions"));
            regions.CreateChildPermission(AppPermissions.Pages_Regions_Create, L("CreateNewRegion"));
            regions.CreateChildPermission(AppPermissions.Pages_Regions_Edit, L("EditRegion"));
            regions.CreateChildPermission(AppPermissions.Pages_Regions_Delete, L("DeleteRegion"));

            #region Flight
            var fligt = fixedVariables.CreateChildPermission(AppPermissions.Pages_Flight, L("Flight"));

            var airports = fligt.CreateChildPermission(AppPermissions.Pages_Airports, L("Airports"));
            airports.CreateChildPermission(AppPermissions.Pages_Airports_Create, L("CreateNewAirport"));
            airports.CreateChildPermission(AppPermissions.Pages_Airports_Edit, L("EditAirport"));
            airports.CreateChildPermission(AppPermissions.Pages_Airports_Delete, L("DeleteAirport"));

            var airlineCompanies = fligt.CreateChildPermission(AppPermissions.Pages_AirlineCompanies, L("AirlineCompanies"));
            airlineCompanies.CreateChildPermission(AppPermissions.Pages_AirlineCompanies_Create, L("CreateNewAirlineCompany"));
            airlineCompanies.CreateChildPermission(AppPermissions.Pages_AirlineCompanies_Edit, L("EditAirlineCompany"));
            airlineCompanies.CreateChildPermission(AppPermissions.Pages_AirlineCompanies_Delete, L("DeleteAirlineCompany"));

            var airportRegions = fligt.CreateChildPermission(AppPermissions.Pages_AirportRegions, L("AirportRegions"));
            airportRegions.CreateChildPermission(AppPermissions.Pages_AirportRegions_Create, L("CreateNewAirportRegion"));
            airportRegions.CreateChildPermission(AppPermissions.Pages_AirportRegions_Edit, L("EditAirportRegion"));
            airportRegions.CreateChildPermission(AppPermissions.Pages_AirportRegions_Delete, L("DeleteAirportRegion"));

            #endregion
            #region Communication
            var communication = fixedVariables.CreateChildPermission(AppPermissions.Pages_Communication, L("Communication"));

            var contacts = communication.CreateChildPermission(AppPermissions.Pages_Contacts, L("Contacts"));
            contacts.CreateChildPermission(AppPermissions.Pages_Contacts_Create, L("CreateNewContact"));
            contacts.CreateChildPermission(AppPermissions.Pages_Contacts_Edit, L("EditContact"));
            contacts.CreateChildPermission(AppPermissions.Pages_Contacts_Delete, L("DeleteContact"));

            var contactDetails = communication.CreateChildPermission(AppPermissions.Pages_ContactDetails, L("ContactDetails"));
            contactDetails.CreateChildPermission(AppPermissions.Pages_ContactDetails_Create, L("CreateNewContactDetail"));
            contactDetails.CreateChildPermission(AppPermissions.Pages_ContactDetails_Edit, L("EditContactDetail"));
            contactDetails.CreateChildPermission(AppPermissions.Pages_ContactDetails_Delete, L("DeleteContactDetail"));

            var contactNetsisDetails = communication.CreateChildPermission(AppPermissions.Pages_ContactNetsisDetails, L("ContactNetsisDetails"));
            contactNetsisDetails.CreateChildPermission(AppPermissions.Pages_ContactNetsisDetails_Create, L("CreateNewContactNetsisDetail"));
            contactNetsisDetails.CreateChildPermission(AppPermissions.Pages_ContactNetsisDetails_Edit, L("EditContactNetsisDetail"));
            contactNetsisDetails.CreateChildPermission(AppPermissions.Pages_ContactNetsisDetails_Delete, L("DeleteContactNetsisDetail"));

            var contactCompanies = communication.CreateChildPermission(AppPermissions.Pages_ContactCompanies, L("ContactCompanies"));
            contactCompanies.CreateChildPermission(AppPermissions.Pages_ContactCompanies_Create, L("CreateNewContactCompany"));
            contactCompanies.CreateChildPermission(AppPermissions.Pages_ContactCompanies_Edit, L("EditContactCompany"));
            contactCompanies.CreateChildPermission(AppPermissions.Pages_ContactCompanies_Delete, L("DeleteContactCompany"));
            #endregion
            #region Payment
            var payment = pages.CreateChildPermission(AppPermissions.Pages_Payment, L("Payment"));

            var contracts = payment.CreateChildPermission(AppPermissions.Pages_Contracts, L("Contracts"));
            contracts.CreateChildPermission(AppPermissions.Pages_Contracts_Create, L("CreateNewContract"));
            contracts.CreateChildPermission(AppPermissions.Pages_Contracts_Edit, L("EditContract"));
            contracts.CreateChildPermission(AppPermissions.Pages_Contracts_Delete, L("DeleteContract"));

            var contractFormules = payment.CreateChildPermission(AppPermissions.Pages_ContractFormules, L("ContractFormules"));
            contractFormules.CreateChildPermission(AppPermissions.Pages_ContractFormules_Create, L("CreateNewContractFormule"));
            contractFormules.CreateChildPermission(AppPermissions.Pages_ContractFormules_Edit, L("EditContractFormule"));
            contractFormules.CreateChildPermission(AppPermissions.Pages_ContractFormules_Delete, L("DeleteContractFormule"));

            var fixedPrices = payment.CreateChildPermission(AppPermissions.Pages_FixedPrices, L("FixedPrices"));
            fixedPrices.CreateChildPermission(AppPermissions.Pages_FixedPrices_Create, L("CreateNewFixedPrice"));
            fixedPrices.CreateChildPermission(AppPermissions.Pages_FixedPrices_Edit, L("EditFixedPrice"));
            fixedPrices.CreateChildPermission(AppPermissions.Pages_FixedPrices_Delete, L("DeleteFixedPrice"));

            var fixedPriceDetails = payment.CreateChildPermission(AppPermissions.Pages_FixedPriceDetails, L("FixedPriceDetails"));
            fixedPriceDetails.CreateChildPermission(AppPermissions.Pages_FixedPriceDetails_Create, L("CreateNewFixedPriceDetail"));
            fixedPriceDetails.CreateChildPermission(AppPermissions.Pages_FixedPriceDetails_Edit, L("EditFixedPriceDetail"));
            fixedPriceDetails.CreateChildPermission(AppPermissions.Pages_FixedPriceDetails_Delete, L("DeleteFixedPriceDetail"));

            var companyTransactions = payment.CreateChildPermission(AppPermissions.Pages_CompanyTransactions, L("CompanyTransactions"));
            companyTransactions.CreateChildPermission(AppPermissions.Pages_CompanyTransactions_Create, L("CreateNewCompanyTransaction"));
            companyTransactions.CreateChildPermission(AppPermissions.Pages_CompanyTransactions_Edit, L("EditCompanyTransaction"));
            companyTransactions.CreateChildPermission(AppPermissions.Pages_CompanyTransactions_Delete, L("DeleteCompanyTransaction"));

            #endregion
            #region Burial
            var burial = pages.CreateChildPermission(AppPermissions.Pages_Burial, L("Burial"));

            var funeralTypes = burial.CreateChildPermission(AppPermissions.Pages_FuneralTypes, L("FuneralTypes"));
            funeralTypes.CreateChildPermission(AppPermissions.Pages_FuneralTypes_Create, L("CreateNewFuneralType"));
            funeralTypes.CreateChildPermission(AppPermissions.Pages_FuneralTypes_Edit, L("EditFuneralType"));
            funeralTypes.CreateChildPermission(AppPermissions.Pages_FuneralTypes_Delete, L("DeleteFuneralType"));

            var funeralFlights = burial.CreateChildPermission(AppPermissions.Pages_FuneralFlights, L("FuneralFlights"));
            funeralFlights.CreateChildPermission(AppPermissions.Pages_FuneralFlights_Create, L("CreateNewFuneralFlight"));
            funeralFlights.CreateChildPermission(AppPermissions.Pages_FuneralFlights_Edit, L("EditFuneralFlight"));
            funeralFlights.CreateChildPermission(AppPermissions.Pages_FuneralFlights_Delete, L("DeleteFuneralFlight"));

            var funerals = burial.CreateChildPermission(AppPermissions.Pages_Funerals, L("Funerals"));
            funerals.CreateChildPermission(AppPermissions.Pages_Funerals_Create, L("CreateNewFuneral"));
            funerals.CreateChildPermission(AppPermissions.Pages_Funerals_Edit, L("EditFuneral"));
            funerals.CreateChildPermission(AppPermissions.Pages_Funerals_Delete, L("DeleteFuneral"));
            funerals.CreateChildPermission(AppPermissions.Pages_Funerals_Funeral_Assignment, L("FuneralsFuneralAssignment"));
            funerals.CreateChildPermission(AppPermissions.Pages_Funerals_Driver_Assignment, L("FuneralsDriverAssignment"));
            funerals.CreateChildPermission(AppPermissions.Pages_Funerals_Vehicle_Assignment, L("FuneralsVehicleAssignment"));

            var funeralDocuments = burial.CreateChildPermission(AppPermissions.Pages_FuneralDocuments, L("FuneralDocuments"));
            funeralDocuments.CreateChildPermission(AppPermissions.Pages_FuneralDocuments_Create, L("CreateNewFuneralDocument"));
            funeralDocuments.CreateChildPermission(AppPermissions.Pages_FuneralDocuments_Edit, L("EditFuneralDocument"));
            funeralDocuments.CreateChildPermission(AppPermissions.Pages_FuneralDocuments_Delete, L("DeleteFuneralDocument"));

            var funeralAddreses = burial.CreateChildPermission(AppPermissions.Pages_FuneralAddreses, L("FuneralAddreses"));
            funeralAddreses.CreateChildPermission(AppPermissions.Pages_FuneralAddreses_Create, L("CreateNewFuneralAddres"));
            funeralAddreses.CreateChildPermission(AppPermissions.Pages_FuneralAddreses_Edit, L("EditFuneralAddres"));
            funeralAddreses.CreateChildPermission(AppPermissions.Pages_FuneralAddreses_Delete, L("DeleteFuneralAddres"));

            var funeralTranportOrders = burial.CreateChildPermission(AppPermissions.Pages_FuneralTranportOrders, L("FuneralTranportOrders"));
            funeralTranportOrders.CreateChildPermission(AppPermissions.Pages_FuneralTranportOrders_Create, L("CreateNewFuneralTranportOrder"));
            funeralTranportOrders.CreateChildPermission(AppPermissions.Pages_FuneralTranportOrders_Edit, L("EditFuneralTranportOrder"));
            funeralTranportOrders.CreateChildPermission(AppPermissions.Pages_FuneralTranportOrders_Delete, L("DeleteFuneralTranportOrder"));

            var funeralPackages = burial.CreateChildPermission(AppPermissions.Pages_FuneralPackages, L("FuneralPackages"));
            funeralPackages.CreateChildPermission(AppPermissions.Pages_FuneralPackages_Create, L("CreateNewFuneralPackage"));
            funeralPackages.CreateChildPermission(AppPermissions.Pages_FuneralPackages_Edit, L("EditFuneralPackage"));
            funeralPackages.CreateChildPermission(AppPermissions.Pages_FuneralPackages_Delete, L("DeleteFuneralPackage"));

            var funeralDriver = burial.CreateChildPermission(AppPermissions.Pages_Funerals_Driver, L("FuneralDriver"));
            funeralDriver.CreateChildPermission(AppPermissions.Pages_Funerals_Driver_Create, L("CreateNewFuneralDriver"));
            funeralDriver.CreateChildPermission(AppPermissions.Pages_Funerals_Driver_Edit, L("EditFuneralDriver"));
            funeralDriver.CreateChildPermission(AppPermissions.Pages_Funerals_Driver_Delete, L("DeleteFuneralDriver"));

            #endregion
            #region Corporation 
            var corporation = pages.CreateChildPermission(AppPermissions.Pages_Corporation, L("Corporation"));

            var companies = corporation.CreateChildPermission(AppPermissions.Pages_Companies, L("Companies"));
            companies.CreateChildPermission(AppPermissions.Pages_Companies_Create, L("CreateNewCompany"));
            companies.CreateChildPermission(AppPermissions.Pages_Companies_Edit, L("EditCompany"));
            companies.CreateChildPermission(AppPermissions.Pages_Companies_Delete, L("DeleteCompany"));

            var vehicles = corporation.CreateChildPermission(AppPermissions.Pages_Vehicles, L("Vehicles"));
            vehicles.CreateChildPermission(AppPermissions.Pages_Vehicles_Create, L("CreateNewVehicle"));
            vehicles.CreateChildPermission(AppPermissions.Pages_Vehicles_Edit, L("EditVehicle"));
            vehicles.CreateChildPermission(AppPermissions.Pages_Vehicles_Delete, L("DeleteVehicle"));

            var companyContacts = corporation.CreateChildPermission(AppPermissions.Pages_CompanyContacts, L("CompanyContacts"));
            companyContacts.CreateChildPermission(AppPermissions.Pages_CompanyContacts_Create, L("CreateNewCompanyContact"));
            companyContacts.CreateChildPermission(AppPermissions.Pages_CompanyContacts_Edit, L("EditCompanyContact"));
            companyContacts.CreateChildPermission(AppPermissions.Pages_CompanyContacts_Delete, L("DeleteCompanyContact"));

            #endregion

            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangeProfilePicture, L("UpdateUsersProfilePicture"));

            var userDetails = administration.CreateChildPermission(AppPermissions.Pages_UserDetails, L("UserDetails"));
            userDetails.CreateChildPermission(AppPermissions.Pages_UserDetails_Create, L("CreateNewUserDetail"));
            userDetails.CreateChildPermission(AppPermissions.Pages_UserDetails_Edit, L("EditUserDetail"));
            userDetails.CreateChildPermission(AppPermissions.Pages_UserDetails_Delete, L("DeleteUserDetail"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeDefaultLanguage, L("ChangeDefaultLanguage"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicProperties = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties, L("DynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Create, L("CreatingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Edit, L("EditingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Delete, L("DeletingDynamicProperties"));

            var dynamicPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue, L("DynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Create, L("CreatingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit, L("EditingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete, L("DeletingDynamicPropertyValue"));

            var dynamicEntityProperties = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties, L("DynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Create, L("CreatingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Edit, L("EditingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Delete, L("DeletingDynamicEntityProperties"));

            var dynamicEntityPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue, L("EntityDynamicPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create, L("CreatingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit, L("EditingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete, L("DeletingDynamicEntityPropertyValue"));

            var massNotification = administration.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification, L("MassNotifications"));
            massNotification.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification_Create, L("MassNotificationCreate"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);

            var maintenance = administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            maintenance.CreateChildPermission(AppPermissions.Pages_Administration_NewVersion_Create, L("SendNewVersionNotification"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TDVConsts.LocalizationSourceName);
        }
    }
}