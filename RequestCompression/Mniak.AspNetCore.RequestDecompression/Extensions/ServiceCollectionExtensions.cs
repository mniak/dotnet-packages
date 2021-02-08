using Mniak.AspNetCore.RequestDecompression;
using Mniak.AspNetCore.RequestDecompression.Abstractions;
using Mniak.AspNetCore.RequestDecompression.Registration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRequestDecompression(this IServiceCollection services)
        {
            services.AddSingleton<RequestDecompressionMiddleware>();
            return services;
        }

        public static IServiceCollection AddRequestDecompressor(this IServiceCollection services, string key, IRequestDecompressor decoder)
        {
            services.AddSingleton(new RequestDecompressorRegistration(key, decoder));
            return services;
        }
    }
}
