using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Example.Client.Infrastructure.Compression
{
    public class GzipContent : HttpContent
    {
        private readonly HttpContent inner;

        public GzipContent(HttpContent? inner, string contentEncoding)
        {
            this.inner = inner ?? throw new ArgumentNullException(nameof(inner));

            // copy original headers
            foreach (var header in this.inner.Headers)
            {
                Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            Headers.ContentEncoding.Add(contentEncoding);
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            using (var compressedStream = new GZipStream(stream, CompressionMode.Compress, leaveOpen: true))
            {
                await inner.CopyToAsync(compressedStream);
            }
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }
    }

}
