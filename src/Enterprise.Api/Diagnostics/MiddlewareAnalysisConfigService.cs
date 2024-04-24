using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.MiddlewareAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace Enterprise.Api.Diagnostics;

internal static class MiddlewareAnalysisConfigService
{
    public static void InsertAnalysisStartupFilter(this WebApplicationBuilder builder)
    {
        if (!builder.Environment.IsDevelopment())
            return;

        // Insert the AnalysisStartupFilter as the first IStartupFilter in the container.
        // https://andrewlock.net/understanding-your-middleware-pipeline-in-dotnet-6-with-the-middleware-analysis-package
        builder.Services.Insert(0, ServiceDescriptor.Transient<IStartupFilter, AnalysisStartupFilter>());
    }

    public static IDisposable? GetDiagnosticListener(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
            return null;

        // Subscribe to the listener with the SubscribeWithAdapter() extension method.
        DiagnosticListener listener = app.Services.GetRequiredService<DiagnosticListener>();

        // Create an instance of the AnalysisDiagnosticAdapter using the IServiceProvider so that the ILogger is injected from DI.
        MiddlewareAnalysisDiagnosticAdapter observer = ActivatorUtilities.CreateInstance<MiddlewareAnalysisDiagnosticAdapter>(app.Services);

        // Grab the "Microsoft.AspNetCore" DiagnosticListener from DI.
        return listener.SubscribeWithAdapter(observer);
    }
}