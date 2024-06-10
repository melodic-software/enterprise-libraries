namespace Enterprise.Options.Monitoring.ChangeNotification.Delegates;

public delegate void OnChange<in TOptions>(TOptions options);
