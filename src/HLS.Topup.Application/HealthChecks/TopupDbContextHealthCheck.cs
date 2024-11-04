using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HLS.Topup.EntityFrameworkCore;

namespace HLS.Topup.HealthChecks
{
    public class TopupDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public TopupDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("TopupDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("TopupDbContext could not connect to database"));
        }
    }
}
