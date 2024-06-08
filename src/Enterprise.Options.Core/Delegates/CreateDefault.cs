namespace Enterprise.Options.Core.Delegates;

public delegate object CreateDefault();
public delegate TOptions CreateDefault<out TOptions>() where TOptions : class, new();
