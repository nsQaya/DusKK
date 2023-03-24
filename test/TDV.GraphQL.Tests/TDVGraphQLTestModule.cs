using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using TDV.Configure;
using TDV.Startup;
using TDV.Test.Base;

namespace TDV.GraphQL.Tests
{
    [DependsOn(
        typeof(TDVGraphQLModule),
        typeof(TDVTestBaseModule))]
    public class TDVGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TDVGraphQLTestModule).GetAssembly());
        }
    }
}