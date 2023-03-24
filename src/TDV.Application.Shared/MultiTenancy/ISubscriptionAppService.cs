using System.Threading.Tasks;
using Abp.Application.Services;

namespace TDV.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
