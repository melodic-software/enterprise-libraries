using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Monitoring.Health.Options.Delegates;

public delegate void AddHealthChecks(IHealthChecksBuilder builder);
