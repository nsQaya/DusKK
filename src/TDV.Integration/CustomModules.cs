using Abp.AspNetCore;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using TDV.Configuration;
using TDV.Integration.Modules.BlobStorage;
using TDV.Integration.Modules.Interfaces;

namespace TDV.Integration
{
    [DependsOn(
        typeof(AbpAspNetCoreModule)
    )]
    public class CustomModules : AbpModule
    {
        public IConfigurationRoot AppConfiguration { get; }
        public IConfigurationSection CustomSection { get; }
        public CustomModules() {
            
            AppConfiguration = AppConfigurations.Get(Directory.GetCurrentDirectory());
        }
        
        public override void PreInitialize()
        {
            /* Register config */
            IocManager.Register<BlobStorageConfig>();
            
            /* Set for blob storage */
            Configuration.Get<BlobStorageConfig>().ConnectionString = AppConfiguration.GetValue<string>("CustomModules:BlobStorage:ConnectionString");
            Configuration.Get<BlobStorageConfig>().MaxSize= AppConfiguration.GetValue<long>("CustomModules:BlobStorage:MaxSize");
            Configuration.Get<BlobStorageConfig>().MaxSizeFriendly = AppConfiguration.GetValue<string>("CustomModules:BlobStorage:MaxSizeFriendly");
            Configuration.Get<BlobStorageConfig>().AllowedMimes = AppConfiguration.GetValue<string[]>("CustomModules:BlobStorage:AllowedMimes");

            Configuration.ReplaceService<IBlobStorageModule, BlobStorageModule>(DependencyLifeStyle.Transient);
        }
    }
}