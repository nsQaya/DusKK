using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AutoMapper;
using AutoMapper.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using TDV.Authorization;
using TDV.Authorization.Mapper;
using TDV.Burial.Mapper;
using TDV.Communication.Mapper;
using TDV.Constants.Mapper;
using TDV.Corporation.Mapper;
using TDV.Integration;
using TDV.Location.Mapper;
using TDV.Payment;
using TDV.Payment.Mapper;

namespace TDV
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(TDVApplicationSharedModule),
        typeof(TDVCoreModule),
        typeof(CustomModules)
    )]
    public class TDVApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {

            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(conf =>
            {
                CustomDtoMapper.CreateMappings(conf);

                var customMappings = new List<Type>() {
                    typeof(CountriesMapper),
                    typeof(CitiesMapper),
                    typeof(RegionsMapper),
                    typeof(DistrictsMapper),
                    typeof(QuartersMapper),
                    typeof(AirportMapper),
                    typeof(AirportRegionMapper),
                    typeof(AirlineCompaniesMapper),
                    typeof(ContactMapper),
                    typeof(ContactDetailMapper),
                    typeof(ContactNetsisDetailMapper),
                    typeof(ContractsMapper),
                    typeof(ContractFormulesMapper),
                    typeof(CompaniesMapper),
                    typeof(VehiclesMapper),
                    typeof(CompanyContactMapper),
                    typeof(FixedPricesMapper),
                    typeof(FixedPriceDetailsMapper),
                    typeof(FuneralsMapper),
                    typeof(FuneralTypesMapper),
                    typeof(FuneralFlightsMapper),
                    typeof(FuneralDocumentsMapper),
                    typeof(FuneralAddressMapper),
                    typeof(FuneralPackagesMapper),
                    typeof(DataListMapper),
                    typeof(FuneralTransportOrdersMapper),
                    typeof(UserDetailsMapper),
                    typeof(DataListMapper),
                    typeof(CurrenciesMapper),
                    typeof(CompanyTransactionsMapper),
                };

                var createMappingArgs = new object[] { conf };

                customMappings.ForEach(x =>
                {
                    x.GetMethod("CreateMapping").Invoke(x, createMappingArgs);
                });

            });

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TDVApplicationModule).GetAssembly());
        }
    }
}