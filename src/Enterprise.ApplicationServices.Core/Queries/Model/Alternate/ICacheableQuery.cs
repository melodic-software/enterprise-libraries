﻿using Enterprise.Library.Core.Attributes;

namespace Enterprise.ApplicationServices.Core.Queries.Model.Alternate;

/// <summary>
/// A query that can be cached.
/// </summary>
/// <typeparam name="TResult"></typeparam>
[AlternativeTo(typeof(ICacheableQuery))]
public interface ICacheableQuery<out TResult> : IQuery<TResult>, ICacheableQuery;