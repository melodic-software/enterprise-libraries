using Enterprise.Logging.Core.Loggers;
using Enterprise.ModularMonoliths.Options;
using Enterprise.Options.Core.Services.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Enterprise.ModularMonoliths.Configuration;

public static class WebApplicationBuilderExtensions
{
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
        
        PreStartupLogger.Instance.LogInformation("Registering module-specific JSON configuration files.");

        IEnumerable<string> jsonSettingsFilePaths = GetJsonSettingsFilePaths(builder.Environment, options);

        foreach (string jsonSettingsFilePath in jsonSettingsFilePaths)
        {
            string fileName = Path.GetFileName(jsonSettingsFilePath);

            if (ConfigIsAlreadyLoaded(builder.Configuration, jsonSettingsFilePath, fileName))
            {
                PreStartupLogger.Instance.LogInformation("Skipping already loaded config file: {JsonSettingsFilePath}", jsonSettingsFilePath);
                continue;
            }

            PreStartupLogger.Instance.LogInformation("Adding json settings file: {JsonSettingsFilePath}", jsonSettingsFilePath);
            builder.Configuration.AddJsonFile(jsonSettingsFilePath);
        }

        PreStartupLogger.Instance.LogInformation("Module specific JSON configuration file registration complete.");

        return builder;
    }

    private static IEnumerable<string> GetJsonSettingsFilePaths(IHostEnvironment hostEnvironment, ModularMonolithOptions options)
    {
        string contentRootPath = hostEnvironment.ContentRootPath;
        // The pattern here should include the module name + any environment qualifiers.
        string searchPattern = $"{options.JsonSettingsFileNameSearchPattern}.json";
        IEnumerable<string> jsonSettingsFilePaths = Directory.EnumerateFiles(contentRootPath, searchPattern, SearchOption.AllDirectories);
        return jsonSettingsFilePaths;
    }

    public static bool ConfigIsAlreadyLoaded(IConfigurationBuilder builder, string filePath, string fileName)
    {
        return builder.Sources.Any(source =>
            source is JsonConfigurationSource { Path: not null } jsonSource &&
            (jsonSource.Path.Equals(filePath, StringComparison.OrdinalIgnoreCase) ||
             jsonSource.Path.Equals(fileName, StringComparison.OrdinalIgnoreCase)
            )
        );
    }
}
