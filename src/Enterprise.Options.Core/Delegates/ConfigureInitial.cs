using Microsoft.Extensions.Configuration;

namespace Enterprise.Options.Core.Delegates;

public delegate object ConfigureInitial(IConfiguration configuration, string? configSectionKey);
