using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TDV.Startup
{
    [DependsOn(typeof(TDVCoreModule))]
    public class TDVGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TDVGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}