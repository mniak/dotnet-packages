using Mniak.AspNetCore.RequestDecompression.Gzip;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGzipDecompression(this IServiceCollection services)
        {
            services.AddRequestDecompression("gzip", new GzipDecompressor());
            return services;
        }
    }
}
