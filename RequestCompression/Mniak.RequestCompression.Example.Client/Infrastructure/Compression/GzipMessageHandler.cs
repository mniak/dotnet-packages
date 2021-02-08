using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Client.Infrastructure.Compression
{
    public class GzipMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Content = new GzipContent(request.Content);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
