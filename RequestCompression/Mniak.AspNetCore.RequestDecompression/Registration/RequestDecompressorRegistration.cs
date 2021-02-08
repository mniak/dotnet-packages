using Mniak.AspNetCore.RequestDecompression.Abstractions;

namespace Mniak.AspNetCore.RequestDecompression.Registration
{
    public record RequestDecompressorRegistration(
        string Key,
        IRequestDecompressor Decoder
    );
}