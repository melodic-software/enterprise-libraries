﻿namespace Example.WebApi.ChainOfResponsibility.Examples.Pipeline.Models;

public class MyRequest
{
    public string Value { get; }

    public MyRequest(string value)
    {
        Value = value;
    }
}
