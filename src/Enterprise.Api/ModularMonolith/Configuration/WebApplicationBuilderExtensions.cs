using Enterprise.Logging.Core.Loggers;
using Enterprise.Options.Core.Services.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.ModularMonolith.Configuration;

public static class WebApplicationBuilderExtensions
{
    private const string Pattern = "*";

    /// <summary>
    /// If the app has been configured as a modular monolith,
    /// this will add any module specific JSON config files that follow the convention.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder RegisterModuleConfig(this WebApplicationBuilder builder)
    {
        ModularMonolithOptions options = OptionsInstanceService.Instance
                .GetOptionsInstance<ModularMonolithOptions>(builder.Configuration, ModularMonolithOptions.ConfigSectionKey);

        if (!options.EnableModularMonolith)
        {
            return builder;
        }
        
        PreStartupLogger.Instance.LogInformation("Registering module specific JSON configuration files.");

        IEnumerable<string> jsonSettingsFiles = GetJsonSettingsFiles(builder.Environment);

        foreach (string jsonSettingsFile in jsonSettingsFiles)
        {
            PreStartupLogger.Instance.LogInformation("Adding json settings file: {JsonSettingsFile}", jsonSettingsFile);
            builder.Configuration.AddJsonFile(jsonSettingsFile);
        }

        PreStartupLogger.Instance.LogInformation("Module specific JSON configuration file registration complete.");

        return builder;
    }

    private static IEnumerable<string> GetJsonSettingsFiles(IHostEnvironment hostEnvironment)
    {
        string contentRootPath = hostEnvironment.ContentRootPath;
        // The pattern here should include the module name + any environment qualifiers.
        string searchPattern = $"module.{Pattern}.json";
        IEnumerable<string> jsonSettingsFiles = Directory.EnumerateFiles(contentRootPath, searchPattern, SearchOption.AllDirectories);
        return jsonSettingsFiles;
    }
}
