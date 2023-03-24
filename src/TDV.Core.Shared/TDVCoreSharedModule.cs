using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TDV
{
    public class TDVCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TDVCoreSharedModule).GetAssembly());
        }
    }
}