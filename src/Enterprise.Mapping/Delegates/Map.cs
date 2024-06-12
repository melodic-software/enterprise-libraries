namespace Enterprise.Mapping.Delegates;

public delegate TResult Map<in TSource, out TResult>(TSource source);
