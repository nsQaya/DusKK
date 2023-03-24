using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace TDV.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
