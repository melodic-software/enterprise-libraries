namespace Enterprise.ModularMonoliths.Exceptions;

public abstract class ModuleException : Exception
{
    protected ModuleException(string message) : base(message)
    {
    }
}
