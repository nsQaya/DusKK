using System.Threading.Tasks;
using Abp.Application.Services;
using TDV.Editions.Dto;
using TDV.MultiTenancy.Dto;

namespace TDV.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}