﻿using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.Events.Facade.Abstract;
using MediatR;

namespace Enterprise.MediatR.Commands.Handlers.Pragmatic;

public abstract class MediatRCommandHandlerBase<TCommand, TResponse>
    : CommandHandlerBase<TCommand, TResponse>, IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    protected MediatRCommandHandlerBase(IEventRaisingFacade eventService) : base(eventService)
    {
    }

    public async Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }
}