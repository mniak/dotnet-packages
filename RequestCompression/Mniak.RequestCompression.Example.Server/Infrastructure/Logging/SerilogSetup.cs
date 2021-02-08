using Destructurama;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Mniak.RequestCompression.Example.Server.Infrastructure.Logging
{
    internal class SerilogSetup
    {
        internal static void ConfigureLogger(HostBuilderContext context, LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration
                .Destructure.JsonNetTypes()
                .WriteTo.Console();
        }
    }
}