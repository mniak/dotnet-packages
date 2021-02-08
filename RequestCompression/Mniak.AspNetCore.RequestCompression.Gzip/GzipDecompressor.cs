using Mniak.AspNetCore.RequestDecompression.Abstractions;
using System.IO;
using System.IO.Compression;

namespace Mniak.AspNetCore.RequestDecompression.Gzip
{
    public class GzipDecompressor : IRequestDecompressor
    {
        public Stream BuildDecompressedStream(Stream compressedStream)
        {
            var stream = new GZipStream(compressedStream, CompressionMode.Decompress, true);
            return stream;
        }
    }
}
