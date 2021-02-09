using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Client.Infrastructure.Compression
{
    public class GzipMessageHandler : DelegatingHandler
    {
        private readonly string contentEncoding;

        public GzipMessageHandler(string contentEncoding = "gzip")
        {
            this.contentEncoding = contentEncoding ?? throw new ArgumentNullException(nameof(contentEncoding));
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Content = new GzipContent(request.Content, contentEncoding);
            return base.SendAsync(request, cancellationToken);
        }

        public GzipMessageHandler WithNewInnerHandler()
        {
            InnerHandler = new HttpClientHandler();
            return this;
        }
    }
}
