﻿namespace Enterprise.Patterns.ResultPattern.Errors.Typed;

public class NoError : Error
{
    public const string GenericCode = "NoError";
    public const ErrorDescriptor Descriptor = ErrorDescriptor.NoError;

    public NoError()
        : base(GenericCode, string.Empty, [Descriptor])
    {
    }
}