using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TDV
{
    [DependsOn(typeof(TDVCoreSharedModule))]
    public class TDVApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TDVApplicationSharedModule).GetAssembly());
        }
    }
}