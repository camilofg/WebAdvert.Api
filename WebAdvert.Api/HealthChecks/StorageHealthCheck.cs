using System.Threading;
using System.Threading.Tasks;
using WebAdvert.Api.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;


namespace WebAdvert.Api.HealthChecks
{
    public class StorageHealthCheck : IHealthCheck
    {
        private readonly IAdvertStorageService _advertStorageService;

        public StorageHealthCheck(IAdvertStorageService advertStorageService)
        {
            _advertStorageService = advertStorageService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var isStorageOk = await _advertStorageService.HealthCheckAsync();
            return await Task.FromResult(isStorageOk ? HealthCheckResult.Healthy("Storage is Healthy") : HealthCheckResult.Unhealthy("Storage is Unhealthy"));
        }
    }
}