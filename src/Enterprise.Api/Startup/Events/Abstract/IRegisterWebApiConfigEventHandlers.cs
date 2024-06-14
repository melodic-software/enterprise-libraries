namespace Enterprise.Api.Startup.Events.Abstract;

public interface IRegisterWebApiConfigEventHandlers
{
    public static abstract void RegisterHandlers(WebApiConfigEvents events);
}
