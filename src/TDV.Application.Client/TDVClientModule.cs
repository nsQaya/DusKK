using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TDV
{
    public class TDVClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TDVClientModule).GetAssembly());
        }
    }
}
