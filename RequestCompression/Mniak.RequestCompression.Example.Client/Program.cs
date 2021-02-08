using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mniak.RequestCompression.Example.Client.Features;
using Mniak.RequestCompression.Example.Client.Infrastructure.Compression;
using Mniak.RequestCompression.Example.Client.Infrastructure.Logging;
using Serilog;

namespace Mniak.RequestCompression.Example.Client
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog(SerilogSetup.ConfigureLogger)
                .ConfigureServices((services) =>
                {
                    services
                        .AddTransient<LoggingMessageHandler>()
                        .AddTransient<GzipMessageHandler>()
                    ;

                    services.AddHttpClient<IMainWorker, MainWorker>()
                        .AddHttpMessageHandler<LoggingMessageHandler>()
                        .AddHttpMessageHandler<GzipMessageHandler>()
                        .AddHttpMessageHandler<GzipMessageHandler>()
                        ;

                    services.AddHostedService<MainService>();
                });
    }
}
