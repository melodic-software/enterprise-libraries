﻿namespace Enterprise.Api.Startup.Options.Abstract;

public interface IConfigureWebApiOptions
{
    public static abstract void Configure(WebApiOptions options);
}