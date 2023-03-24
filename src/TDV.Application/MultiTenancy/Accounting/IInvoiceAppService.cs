using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using TDV.MultiTenancy.Accounting.Dto;

namespace TDV.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
