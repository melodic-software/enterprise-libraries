﻿using Enterprise.ApplicationServices.Core.Commands.Model;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Delegates;

public delegate void ConfigureOptions<TCommand>(RegistrationOptions<TCommand> options) where TCommand : IBaseCommand;