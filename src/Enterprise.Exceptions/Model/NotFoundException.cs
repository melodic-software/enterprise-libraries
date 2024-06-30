namespace Enterprise.Exceptions.Model;

public class NotFoundException : Exception
{
    protected NotFoundException(string message) : base(message)
    {
    }
}