using Destructurama;
using Serilog;
using Serilog.Context;
using Serilog.Formatting.Json;

try
{
    // A custom buffer size can be set. The default is 10,000.
    // The JsonFormatter is required for LogContext properties (in the console sink).

    ILogger logger = new LoggerConfiguration()
        //.WriteTo.Async(x => x.Console(theme: AnsiConsoleTheme.Code))
        .WriteTo.Async(x => x.Console(new JsonFormatter()))
        .Enrich.FromLogContext()
        .Destructure.UsingAttributes()
        //.Destructure.ByTransforming<T>(x => new
        //{
        //    x.Property1, x.Property2
        //})
        .CreateLogger();

    Log.Logger = logger;

    logger.Information("Hello, World!");

    var data = new
    {
        Property1 = "value",
        Property2 = DateTime.UtcNow
    };

    using (LogContext.PushProperty("Property3", "SomeValue"))
    {
        logger.Information("This is a test with structured data: {@Data}.", data);
        logger.Information("This is a test with explicit ToString qualifier: {$Property1}.", data.Property1);
        logger.Information("This is a formatting test: {Property2:O}.", data.Property2);
    }

    //throw new Exception("TEST");

    Console.ReadLine();
}
catch (Exception ex)
{
    Log.Error(ex, "An error occurred.");
    throw;
}
finally
{
    // Be sure to do this before the process ends.
    Log.CloseAndFlush();

    // Adding this so the logs can be viewed.
    Thread.Sleep(5000);
}