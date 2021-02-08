using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace Mniak.RequestCompression.Example.Server.Infrastructure.Compression
{
    public class GzipRequestMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            using (var multiDisposable = new MultiDisposable())
            {
                while (IsGzip(context, out var encodings))
                {
                    var stream = multiDisposable.Add(new GZipStream(context.Request.Body, CompressionMode.Decompress, true));
                    context.Request.Headers["Content-Encoding"] = new StringValues(encodings.SkipLast(1).ToArray());
                    context.Request.Body = stream;
                }
                return next(context);
            }
        }

        private static bool IsGzip(HttpContext context, out IEnumerable<string> encodings)
        {
            encodings = context.Request.Headers["Content-Encoding"]
               .SelectMany(x => x.Split(','))
               .Select(x => x.Trim().ToLowerInvariant());

            return encodings.LastOrDefault() == "gzip";
        }
    }
}
