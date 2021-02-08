using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Mniak.AspNetCore.RequestDecompression.Abstractions;
using Mniak.AspNetCore.RequestDecompression.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mniak.AspNetCore.RequestDecompression
{
    public class RequestDecompressionMiddleware : IMiddleware
    {
        private readonly ILogger logger;
        private readonly IDictionary<string, IRequestDecompressor> knownEncodings;

        public RequestDecompressionMiddleware(
            ILogger<RequestDecompressionMiddleware> logger,
            IEnumerable<RequestDecompressorRegistration> registrations)
        {
            if (registrations is null)
                throw new ArgumentNullException(nameof(registrations));

            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            knownEncodings = registrations.GroupBy(r => r.Key)
                .ToDictionary(x => x.Key, x => x.Last().Decoder);
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            using (var multiDisposable = new MultiDisposable())
            {
                while (IsKnownEncoding(context, out var encodingKeys, out var decoder) && decoder != null)
                {
                    var stream = multiDisposable.Add(decoder.BuildDecompressedStream(context.Request.Body));
                    context.Request.Headers["Content-Encoding"] = new StringValues(encodingKeys.SkipLast(1).ToArray());
                    context.Request.Body = stream;
                }
                await next(context);
            }
        }

        private bool IsKnownEncoding(HttpContext context, out IEnumerable<string> encodingKeys, out IRequestDecompressor? decoder)
        {
            encodingKeys = context.Request.Headers["Content-Encoding"]
               .SelectMany(x => x.Split(','))
               .Select(x => x.Trim().ToLowerInvariant());

            if (!encodingKeys.Any())
            {
                decoder = null;
                return false;
            }

            var contentEncodingKey = encodingKeys.Last();
            var result = knownEncodings.ContainsKey(contentEncodingKey);
            if (!result)
            {
                logger.LogDebug("The server does not know how to handle the content encoding '{ContentEncoding}'", contentEncodingKey);
                decoder = null;
                return false;
            }

            decoder = knownEncodings[contentEncodingKey];
            return true;
        }
    }
}
