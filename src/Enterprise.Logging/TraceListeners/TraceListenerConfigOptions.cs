﻿using Enterprise.Logging.Options;

namespace Enterprise.Logging.TraceListeners;

public class TraceListenerConfigOptions
{
    public const string ConfigSectionKey = "Custom:Logging:TraceListeners";

    public bool EnableTextFileTraceListener { get; set; } = false;

    /// <summary>
    /// The name of the application that is safe to use for file system operations.
    /// Folder names, file names, etc.
    /// </summary>
    public string LogFileApplicationName { get; set; } = LoggingConfigOptionDefaults.LogFileApplicationName;
}