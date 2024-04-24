namespace Example.ConsoleApp;

public class Application
{
    public Task RunAsync(string[] args)
    {
        Console.WriteLine("Running app...");
        Console.ReadKey(true);
        return Task.CompletedTask;
    }
}