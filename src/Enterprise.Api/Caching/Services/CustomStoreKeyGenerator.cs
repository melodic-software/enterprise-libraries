﻿using Marvin.Cache.Headers;
using Marvin.Cache.Headers.Domain;
using Marvin.Cache.Headers.Interfaces;
using Microsoft.Extensions.Primitives;
using static Enterprise.Api.Versioning.Constants.VersioningConstants;
using static Enterprise.Constants.CharacterConstants;

namespace Enterprise.Api.Caching.Services;
#nullable disable

public class CustomStoreKeyGenerator : IStoreKeyGenerator
{
    private readonly DefaultStoreKeyGenerator _defaultStoreKeyGenerator;

    private const string QueryStringKey = "queryString";
    private const string ResourcePathKey = "resourcePath";
    private const string RequestHeaderValuesKey = "requestHeaderValues";

    public CustomStoreKeyGenerator()
    {
        _defaultStoreKeyGenerator = new DefaultStoreKeyGenerator();
    }

    public async Task<StoreKey> GenerateStoreKey(StoreKeyContext context)
    {
        StoreKey storeKey = await EnrichDefaultStoreKeyAsync(context);
        //StoreKey customStoreKey = await GenerateCustomStoreKey(context);
        return storeKey;
    }

    private async Task<StoreKey> EnrichDefaultStoreKeyAsync(StoreKeyContext context)
    {
        StoreKey storeKey = await _defaultStoreKeyGenerator.GenerateStoreKey(context);

        // if a versioning header value is present, we need to include it in the store key
        bool versionHeaderParsed = context.HttpRequest.Headers.TryGetValue(CustomVersionRequestHeader, out StringValues versionHeaderValue);

        if (!versionHeaderParsed)
        {
            return storeKey;
        }

        if (!storeKey.TryGetValue(RequestHeaderValuesKey, out string requestHeaderValues))
        {
            return storeKey;
        }

        requestHeaderValues += $"{Dash}{versionHeaderValue}";
        storeKey[RequestHeaderValuesKey] = requestHeaderValues;

        return storeKey;
    }

    private Task<StoreKey> GenerateCustomStoreKey(StoreKeyContext context)
    {
        List<string> values = context.VaryByAll
            ? context.HttpRequest.Headers.SelectMany(h => h.Value).ToList()
            : context.HttpRequest.Headers
                .Where(x => context.Vary.Any(h => h.Equals(x.Key, StringComparison.OrdinalIgnoreCase)))
                .SelectMany(h => h.Value).ToList();

        string path = context.HttpRequest.Path.ToString();
        string queryString = context.HttpRequest.QueryString.ToString();

        // if a versioning header value is present, we need to include it in the store key
        bool versionHeaderParsed = context.HttpRequest.Headers.TryGetValue(CustomVersionRequestHeader, out StringValues versionHeaderValue);

        if (versionHeaderParsed)
        {
            values.Add(versionHeaderValue);
        }

        var storeKey = new StoreKey
        {
            { QueryStringKey, path },
            { ResourcePathKey, queryString },
            { RequestHeaderValuesKey, string.Join(Dash, values) }
        };

        return Task.FromResult(storeKey);
    }
}
