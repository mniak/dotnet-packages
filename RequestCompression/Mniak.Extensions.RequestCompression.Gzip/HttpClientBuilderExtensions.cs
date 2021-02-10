using Mniak.HttpClient.RequestCompression.Gzip;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddGzipCompression(this IHttpClientBuilder builder, string contentEncoding = "gzip")
        {
            builder.AddHttpMessageHandler(() => new GzipMessageHandler(contentEncoding));
            return builder;
        }
    }
}
