using Enterprise.Options.Core.Singleton;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace Enterprise.Logging.TraceListeners;

internal static class TraceListenerConfigService
{
    internal static void ConfigureTraceListeners(this IHostApplicationBuilder builder)
    {
        TraceListenerConfigOptions configOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<TraceListenerConfigOptions>(builder.Configuration, TraceListenerConfigOptions.ConfigSectionKey);

        builder.ConfigureTraceListeners(configOptions);
    }

    internal static void ConfigureTraceListeners(this IHostApplicationBuilder builder, TraceListenerConfigOptions configOptions)
    {
        if (!configOptions.EnableTextFileTraceListener)
            return;

        ArgumentException.ThrowIfNullOrWhiteSpace(configOptions.LogFileApplicationName);

        // Mac: /Users/<username>/.local/share
        // Windows: C:\Users\<username>\AppData\local
        string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string applicationName = configOptions.LogFileApplicationName;
        DateTime timestamp = DateTime.Now;
        string tracePath = Path.Join(localAppDataPath, $"Log_{applicationName}_{timestamp:yyyMMdd-HHmm}.txt");
        StreamWriter fileStreamWriter = File.CreateText(tracePath);
        TextWriterTraceListener textWriterTraceListener = new TextWriterTraceListener(fileStreamWriter);
        Trace.Listeners.Add(textWriterTraceListener);
        Trace.AutoFlush = true;
    }
}