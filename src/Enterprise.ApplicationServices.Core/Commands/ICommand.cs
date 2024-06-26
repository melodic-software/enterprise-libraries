﻿using Enterprise.Patterns.ResultPattern.Model;
using MediatR;

namespace Enterprise.ApplicationServices.Core.Commands;

/// <summary>
/// This is a marker interface that signifies that an implementing class is a command object.
/// It is used primarily for constraint purposes.
/// </summary>
public interface ICommand : IRequest<Result>, IBaseCommand;

/// <summary>
/// This is a marker interface that signifies that an implementing class is a command object.
/// It is used primarily for constraint purposes.
/// According to Bertrand Meyer, commands don't have a return value. They return void.
/// This interface is a pragmatic compromise that allows for defining a result associated with the command.
/// Use of this interface is acceptable; however, <see cref="ICommand"/> is preferred.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface ICommand<TResponse> : IBaseCommand, IRequest<Result<TResponse>>;