using Mniak.AspNetCore.RequestDecompression;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestDecompression(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<RequestDecompressionMiddleware>();
            return builder;
        }
    }
}
