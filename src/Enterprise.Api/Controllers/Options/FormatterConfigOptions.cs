﻿using Microsoft.AspNetCore.Mvc.Formatters;

namespace Enterprise.Api.Controllers.Options;

public class FormatterConfigOptions
{
    public List<IInputFormatter> InputFormatters { get; set; } = [];
    public List<IOutputFormatter> OutputFormatters { get; set; } = [];
}