using Destructurama;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Mniak.RequestCompression.Example.Client.Infrastructure.Logging
{
    internal static class SerilogSetup
    {
        internal static void ConfigureLogger(HostBuilderContext context, LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration
                .Destructure.JsonNetTypes()
                .WriteTo.Console();
        }
    }
}