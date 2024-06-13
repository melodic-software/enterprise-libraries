using Enterprise.Api.Startup.Events;

namespace Enterprise.Api.Core.Startup.Events.Abstract;

public interface IRegisterWebApiConfigEventHandlers
{
    public static abstract void RegisterHandlers(WebApiConfigEvents events);
}
