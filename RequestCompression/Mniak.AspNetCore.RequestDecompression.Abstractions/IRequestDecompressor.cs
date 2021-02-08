using System.IO;

namespace Mniak.AspNetCore.RequestDecompression.Abstractions
{
    public interface IRequestDecompressor
    {
        Stream BuildDecompressedStream(Stream compressedStream);
    }
}
