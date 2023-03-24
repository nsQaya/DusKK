using Abp.Domain.Services;

namespace TDV
{
    public abstract class TDVDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected TDVDomainServiceBase()
        {
            LocalizationSourceName = TDVConsts.LocalizationSourceName;
        }
    }
}
