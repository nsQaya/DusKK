using System.Threading.Tasks;
using Abp.Application.Services;
using TDV.Sessions.Dto;

namespace TDV.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
