using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mniak.RequestCompression.Example.Client.Infrastructure.Compression
{
    public class GzipContent : HttpContent
    {
        public const string ContentEncoding = "gzip";
        private readonly HttpContent inner;

        public GzipContent(HttpContent inner)
        {
            this.inner = inner;

            // copy original headers
            foreach (var header in this.inner.Headers)
            {
                Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            Headers.ContentEncoding.Add(ContentEncoding);
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
