using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using Enterprise.Options.Core.Services.Singleton;

namespace Enterprise.Logging.TraceListeners;

internal static class TraceListenerConfigService
{
    internal static void ConfigureTraceListeners(this IHostApplicationBuilder builder)
    {
        TraceListenerConfigOptions configOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<TraceListenerConfigOptions>(builder.Configuration, TraceListenerConfigOptions.ConfigSectionKey);

        ConfigureTraceListeners(configOptions);
    }

    internal static void ConfigureTraceListeners(TraceListenerConfigOptions configOptions)
    {
        if (!configOptions.EnableTextFileTraceListener)
        {
            return;
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(configOptions.LogFileApplicationName);

        // Mac: /Users/<username>/.local/share
        // Windows: C:\Users\<username>\AppData\local
        string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string applicationName = configOptions.LogFileApplicationName;
        DateTime timestamp = DateTime.Now;
        string tracePath = Path.Join(localAppDataPath, $"Log_{applicationName}_{timestamp:yyyMMdd-HHmm}.txt");
        StreamWriter fileStreamWriter = File.CreateText(tracePath);
        var textWriterTraceListener = new TextWriterTraceListener(fileStreamWriter);
        Trace.Listeners.Add(textWriterTraceListener);
        Trace.AutoFlush = true;
    }
}
