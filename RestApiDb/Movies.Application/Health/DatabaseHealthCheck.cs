using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Movies.Application.Database;

namespace Movies.Application.Health;

public class DatabaseHealthCheck(
    IDbConnectionFactory dbConnectionFactory,
    ILogger<DatabaseHealthCheck> logger) 
    : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
        CancellationToken cancellationToken)
    {
        try
        {
            _ = await dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            return HealthCheckResult.Healthy();

        }
        catch (Exception e)
        {
            const string errorMessage = $"Database is unhealthy";
            logger.LogError(errorMessage, e);
            return HealthCheckResult.Unhealthy(errorMessage, e);
        }
    }
}