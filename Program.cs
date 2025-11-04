using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Serilog;

using SerilogWithILoggerLogs;

string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{environment}.json", true, true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Starting!");

    using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
    {
        builder.ClearProviders();
        builder.AddSerilog(dispose: true);
    });

    ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
    logger.LogInformation("Hello from {App}!", "Serilog + ILogger sample");
    logger.LogWarning("This is a sample warning at {Timestamp}", DateTimeOffset.Now);
    logger.LogError("This is a sample error at {Timestamp}", DateTimeOffset.Now);

    Service service = new(loggerFactory.CreateLogger<Service>());
    service.PerformAction();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.Information("Shutting down...");
    Log.CloseAndFlush();
}