using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TDV.EntityFrameworkCore;

namespace TDV.HealthChecks
{
    public class TDVDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public TDVDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("TDVDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("TDVDbContext could not connect to database"));
        }
    }
}
