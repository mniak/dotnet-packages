using Destructurama;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Example.Client.Infrastructure.Logging
{
    internal static class SerilogSetup
    {
        internal static void ConfigureLogger(HostBuilderContext context, LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration
                .MinimumLevel.Debug()
                .Destructure.JsonNetTypes()
                .WriteTo.Console();
        }
    }
}