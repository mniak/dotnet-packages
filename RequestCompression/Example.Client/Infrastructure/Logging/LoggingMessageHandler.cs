using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Client.Infrastructure.Logging
{
    public class LoggingMessageHandler : DelegatingHandler
    {
        private const string LogPrefix = "[HttpClient] ";
        internal readonly ILogger logger;

        public LoggingMessageHandler(ILogger<LoggingMessageHandler> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await LogRequestAsync(request);
            var response = await base.SendAsync(request, cancellationToken);
            await LogResponseAsync(response);
            return response;
        }

        private async Task LogRequestAsync(HttpRequestMessage request)
        {
            logger.LogInformation(LogPrefix + @"{Method} {Url}
Headers: {@Headers}
Content: {@Content}",
                request.Method,
                request.RequestUri,
                FormatHeaders(request.Headers),
                await FormatContentAsync(request.Content));
        }
        private async Task LogResponseAsync(HttpResponseMessage response)
        {
            logger.LogInformation(LogPrefix + @"Response {StatusCode} {StatusReason}
Headers: {@Headers}
Content: {@Content}",
                (int)response.StatusCode, response.ReasonPhrase,
                FormatHeaders(response.Headers),
                await FormatContentAsync(response.Content));
        }

        private IDictionary<string, string> FormatHeaders(HttpHeaders headers)
        {
            if (headers == null)
                return default;

            return headers
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, x => string.Join(",", x.SelectMany(y => y.Value)));
        }
        private async Task<object> FormatContentAsync(HttpContent content)
        {
            if (content == null)
                return null;

            var str = await content.ReadAsStringAsync();
            return TryParseToJson(str);
        }

        private object TryParseToJson(string text)
        {
            try { return JToken.Parse(text); }
            catch { return text; }
        }
    }
}
