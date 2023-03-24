using System.Threading.Tasks;
using TDV.Sessions.Dto;

namespace TDV.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
