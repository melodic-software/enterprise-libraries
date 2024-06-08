namespace Enterprise.Options.Core.Delegates;

public delegate void Configure(object options);
public delegate void Configure<in TOptions>(TOptions options) where TOptions : class, new();
