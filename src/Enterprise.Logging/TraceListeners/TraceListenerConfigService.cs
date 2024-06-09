using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using Enterprise.Options.Core.Services.Singleton;

namespace Enterprise.Logging.TraceListeners;

internal static class TraceListenerConfigService
{
    internal static void ConfigureTraceListeners(this IHostApplicationBuilder builder)
    {
        TraceListenerOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<TraceListenerOptions>(builder.Configuration, TraceListenerOptions.ConfigSectionKey);

        ConfigureTraceListeners(options);
    }

    internal static void ConfigureTraceListeners(TraceListenerOptions options)
    {
        if (!options.EnableTextFileTraceListener)
        {
            return;
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(options.LogFileApplicationName);

        // Mac: /Users/<username>/.local/share
        // Windows: C:\Users\<username>\AppData\local
        string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string applicationName = options.LogFileApplicationName;
        DateTime timestamp = DateTime.Now;
        string tracePath = Path.Join(localAppDataPath, $"Log_{applicationName}_{timestamp:yyyMMdd-HHmm}.txt");
        StreamWriter fileStreamWriter = File.CreateText(tracePath);
        var textWriterTraceListener = new TextWriterTraceListener(fileStreamWriter);
        Trace.Listeners.Add(textWriterTraceListener);
        Trace.AutoFlush = true;
    }
}
