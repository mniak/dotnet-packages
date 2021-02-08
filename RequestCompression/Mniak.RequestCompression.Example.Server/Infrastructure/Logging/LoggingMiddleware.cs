namespace Mniak.RequestCompression.Example.Server.Infrastructure.Logging
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.Extensions.Logging;
    using Microsoft.IO;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    namespace Cielo.Api.Infrastructure.Logging
    {
        internal class LoggingMiddleware : IMiddleware
        {
            private const string LOG_PREFIX = "[API] ";
            private readonly ILogger logger;
            private readonly RecyclableMemoryStreamManager recyclableMemoryStreamManager;

            public LoggingMiddleware(ILogger<LoggingMiddleware> logger)
            {
                this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
                recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
            }

            public async Task InvokeAsync(HttpContext context, RequestDelegate next)
            {
                async Task callNext(HttpContext ctx)
                {
                    try
                    {
                        await next.Invoke(ctx);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Unhandled Error");
                        throw;
                    }
                }

                await LogRequestAsync(context.Request);
                await LogResponseAsync(context, callNext);
            }

            private async Task LogRequestAsync(HttpRequest request)
            {
                logger.LogInformation(LOG_PREFIX + "Received request {HttpMethod} {Url} with headers {@Headers} and body {@Body}",
                    request.Method,
                    request.GetDisplayUrl(),
                    FormatHeaders(request.Headers),
                    await ReadRequestBodyAsync(request));
            }

            private async Task LogResponseAsync(HttpContext context, RequestDelegate next)
            {
                var originalBody = context.Response.Body;
                using (var newBody = recyclableMemoryStreamManager.GetStream())
                {
                    context.Response.Body = newBody;
                    await next(context);

                    var responseBodyContent = await ReadResponseBodyAsync(context.Response);
                    await newBody.CopyToAsync(originalBody);

                    logger.LogInformation(LOG_PREFIX + "Sending response {HttpStatus} with headers {@Headers} and body {@Body}",
                        context.Response.StatusCode,
                        FormatHeaders(context.Response.Headers),
                        responseBodyContent);
                }
            }

            private object FormatHeaders(IHeaderDictionary headers)
            {
                var result = headers
                    .ToDictionary(x => x.Key, x => string.Join(',', x.Value));
                return result;
            }
            private async Task<object> ReadRequestBodyAsync(HttpRequest request)
            {
                request.EnableBuffering();
                using var streamReader = new StreamReader(request.Body, leaveOpen: true);
                var text = await streamReader.ReadToEndAsync();
                request.Body.Seek(0, SeekOrigin.Begin);
                return ParseJson(text);
            }
            private async Task<object> ReadResponseBodyAsync(HttpResponse response)
            {
                response.Body.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(response.Body, leaveOpen: true);
                var text = await reader.ReadToEndAsync();
                response.Body.Seek(0, SeekOrigin.Begin);
                return ParseJson(text);
            }

            private object ParseJson(string text)
            {
                try
                {
                    return JToken.Parse(text);
                }
                catch (JsonReaderException)
                {
                    return text;
                }
            }
        }
    }
}
