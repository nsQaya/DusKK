using Abp;

namespace TDV
{
    /// <summary>
    /// This class can be used as a base class for services in this application.
    /// It has some useful objects property-injected and has some basic methods most of services may need to.
    /// It's suitable for non domain nor application service classes.
    /// For domain services inherit <see cref="TDVDomainServiceBase"/>.
    /// For application services inherit TDVAppServiceBase.
    /// </summary>
    public abstract class TDVServiceBase : AbpServiceBase
    {
        protected TDVServiceBase()
        {
            LocalizationSourceName = TDVConsts.LocalizationSourceName;
        }
    }
}