using System.Threading.Tasks;
using Abp.Dependency;

namespace TDV.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}