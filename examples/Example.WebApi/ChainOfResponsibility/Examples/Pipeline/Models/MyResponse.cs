﻿namespace Example.WebApi.ChainOfResponsibility.Examples.Pipeline.Models;

public class MyResponse
{
    public bool Value { get; }

    public MyResponse(bool value)
    {
        Value = value;
    }
}
