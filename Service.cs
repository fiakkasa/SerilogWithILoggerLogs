using Microsoft.Extensions.Logging;

namespace SerilogWithILoggerLogs;

public class Service(ILogger<Service> logger)
{
    public void PerformAction()
    {
        logger.LogInformation(
            "Hello from Service running at {Timestamp} method {MethodName}",
            DateTimeOffset.Now,
            nameof(PerformAction)
        );
    }
}