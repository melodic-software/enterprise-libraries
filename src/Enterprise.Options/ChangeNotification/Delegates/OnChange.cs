namespace Enterprise.Options.ChangeNotification.Delegates;

public delegate void OnChange<in TOptions>(TOptions options);
