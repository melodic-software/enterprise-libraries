namespace Enterprise.Options.Core.Delegates;

public delegate void ApplyChanges<in TOptions>(TOptions options) where TOptions : class, new();
