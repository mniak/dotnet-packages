using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Mniak.RequestCompression.Example.Server.Infrastructure.Logging;
using Serilog;

namespace Mniak.RequestCompression.Example.Server
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
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
